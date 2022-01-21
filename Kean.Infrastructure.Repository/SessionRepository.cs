using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using Kean.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 会话仓库
    /// </summary>
    public class SessionRepository :
        Domain.Identity.Repositories.ISessionRepository
    {
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SessionRepository(
            IDefaultRedis redis)
        {
            _redis = redis;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.LoadKeys() 方法
         */
        public async Task<IEnumerable<string>> LoadKeys()
        {
            var identities = await _redis.Hash["identity:index"].Range();
            if (identities.Any())
            {
                var details = await _redis.Batch(batch => batch.Execute(identities.Select(i => _redis.Hash[$"identity:{i.Key}"].Range()).ToArray()));
                return details.SelectMany(d => d.Keys).Where(k => k.StartsWith("session")).Select(k => k[8..]);
            }
            else
            {
                return Array.Empty<string>();
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.CreateSession(string key, int user) 方法
         */
        public async Task CreateSession(string key, int identity)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await _redis.Hash["identity:index"].Set(identity.ToString(), timestamp);
            await _redis.Hash[$"identity:{identity}"].Set($"session_{key}", timestamp);
            await _redis.Hash[$"session:{key}"].Set("identity", identity.ToString());
            await _redis.Hash[$"session:{key}"].Set("timestamp", timestamp);
            var super = await _redis.Hash["param"].Get("super_user");
            if (super != null)
            {
                var user = JsonHelper.Deserialize<T_SYS_USER>(super);
                if (identity == user.USER_ID)
                {
                    await _redis.Hash[$"identity:{identity}"].Set("tag", "super");
                }
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.UpdateSession(string key) 方法
         */
        public async Task UpdateSession(string key)
        {
            await _redis.Hash[$"session:{key}"].Set("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.RemoveSession(string key) 方法
         */
        public async Task RemoveSession(string key)
        {
            var identity = await _redis.Hash[$"session:{key}"].Get("identity");
            await _redis.Hash[$"identity:{identity}"].Set($"session_{key}", null);
            await _redis.String[$"session:{key}"].Set(null);
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.IsTimeout(string key) 方法
         */
        public async Task<bool> IsTimeout(string key)
        {
            if (DateTime.TryParse(await _redis.Hash[$"session:{key}"].Get("timestamp"), out var timestamp))
            {
                if (int.TryParse(await _redis.Hash["param"].Get("session_timeout"), out var timeout) && timeout > 0)
                {
                    return DateTime.Now.Subtract(timestamp).TotalMinutes > timeout;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.IsPasswordInitial(string key) 方法
         */
        public async Task<bool> IsPasswordInitial(string key)
        {
            var identity = await _redis.Hash[$"session:{key}"].Get("identity");
            return await _redis.Hash[$"identity:{identity}"].Get("password_initial") != "0";
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.IsPasswordExpired(string key) 方法
         */
        public async Task<bool> IsPasswordExpired(string key)
        {
            var identity = await _redis.Hash[$"session:{key}"].Get("identity");
            return DateTime.TryParse(await _redis.Hash[$"identity:{identity}"].Get("password_expired"), out var expired) && DateTime.Now > expired;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.HasPermission(string key, string url) 方法
         */
        public async Task<bool> HasPermission(string key, string url)
        {
            var identity = await _redis.Hash[$"session:{key}"].Get("identity");
            var hash = _redis.Hash[$"identity:{identity}"];
            if (await hash.Get("tag") == "super")
            {
                return true;
            }
            else
            {
                return await hash.Get($"url_{url}") == await hash.Get("url_version");
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.GetIdentity(string key) 方法
         */
        public async Task<int?> GetIdentity(string key)
        {
            if (int.TryParse(await _redis.Hash[$"session:{key}"].Get("identity"), out var identity))
            {
                return identity;
            }
            else
            {
                return null;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.GetConflicts(string key) 方法
         */
        public async Task<IEnumerable<string>> GetConflicts(string key)
        {
            if (await _redis.Hash["param"].Get("session_mode") == "Single" && int.TryParse(await _redis.Hash[$"session:{key}"].Get("identity"), out var identity))
            {
                return (await _redis.Hash[$"identity:{identity}"].Range())
                    .Where(s => s.Key != $"session:{key}" && s.Key.StartsWith("session:"))
                    .Select(s => s.Key[8..]);
            }
            else
            {
                return null;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.GetConnection(string key) 方法
         */
        public Task<string> GetConnection(string key)
        {
            return _redis.Hash[$"session:{key}"].Get("connection");
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.RegisterConnection(string key, string id) 方法
         */
        public async Task<bool> RegisterConnection(string key, string id)
        {
            var hash = _redis.Hash[$"session:{key}"];
            if (await hash.Get("identity") == null)
            {
                return false;
            }
            else
            {
                await hash.Set("connection", id);
                return true;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISessionRepository.UnregisterConnection(string key, string id) 方法
         */
        public async Task<bool> UnregisterConnection(string key, string id)
        {
            var hash = _redis.Hash[$"session:{key}"];
            if (await hash.Get("identity") == null || await hash.Get("connection") != id)
            {
                return false;
            }
            else
            {
                await hash.Set("connection", null);
                return true;
            }
        }
    }
}
