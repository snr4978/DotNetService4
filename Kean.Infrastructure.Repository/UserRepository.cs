using AutoMapper;
using Kean.Domain.Basic.Models;
using Kean.Domain.Identity.Models;
using Kean.Infrastructure.Database;
using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using Kean.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 用户仓库
    /// </summary>
    public class UserRepository :
        Domain.Identity.Repositories.IUserRepository,
        Domain.Basic.Repositories.IUserRepository
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultDb _database; // 默认数据库
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserRepository(
            IMapper mapper,
            IDefaultDb database,
            IDefaultRedis redis)
        {
            _mapper = mapper;
            _database = database;
            _redis = redis;
        }

        #region Kean.Domain.Identity.Repositories.IUserRepository 接口实现部分

        /*
         * 根据给定的用户信息判断该用户是否是超级用户，并获取完整的超级用户信息
         */
        private async Task<T_SYS_USER> Super(Func<T_SYS_USER, bool> func)
        {
            var json = await _redis.Hash["param"].Get("super_user");
            var super = json == null ? null : JsonHelper.Deserialize<T_SYS_USER>(json);
            return super != null && func(super) ? super : null;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.GetIdentity(string account, string password) 方法
         */
        public async Task<int?> GetIdentity(string account, Password password)
        {
            return ((await Super(u => u.USER_ACCOUNT == account && u.USER_PASSWORD == password.CipherText)) ?? 
                (await _database.From<T_SYS_USER>()
                .Where(u => u.USER_ACCOUNT == account && u.USER_PASSWORD == password.CipherText)
                .Single()))?
                .USER_ID;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.PasswordInitial(int id) 方法
         */
        public async Task<bool?> PasswordInitial(int id)
        {
            var param = _redis.Hash["param"];
            var identity = _redis.Hash[$"identity:{id}"];
            if (await param.Get("password_initial") == "Enable")
            {
                var @default = await param.Get("default_password");
                var password = (await _database.From<T_SYS_USER>()
                    .Where(u => u.USER_ID == id)
                    .Single())?
                    .USER_PASSWORD;
                if (password == (@default == null ? null : new Password(@default)))
                {
                    await identity.Set("password_initial", "0");
                    return false;
                }
                else
                {
                    await identity.Set("password_initial", "1");
                    return true;
                }
            }
            else
            {
                await identity.Set("password_initial", null);
                return null;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.PasswordExpired(int id) 方法
         */
        public async Task<DateTime?> PasswordExpired(int id)
        {
            var param = _redis.Hash["param"];
            var identity = _redis.Hash[$"identity:{id}"];
            if (int.TryParse(await param.Get("password_timeout"), out int days) && days > 0)
            {
                var date = (await _database.From<T_SYS_USER>()
                    .Where(u => u.USER_ID == id).Single())?
                    .USER_PASSWORD_TIME?.AddDays(days);
                await identity.Set("password_expired", date?.ToString("yyyy-MM-dd HH:mm:ss"));
                return date;
            }
            else
            {
                await identity.Set("password_expired", null);
                return null;
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.VerifyPassword(int id, Password password) 方法
         */
        public async Task<bool> VerifyPassword(int id, Password password)
        {
            return ((await Super(u => u.USER_ID == id)) ??
                (await _database.From<T_SYS_USER>()
                .Where(u => u.USER_ID == id)
                .Single()))?
                .USER_PASSWORD == password;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.ModifyPassword(int id, Password password) 方法
         */
        public async Task<bool> ModifyPassword(int id, Password password)
        {
            var param = _redis.Hash["param"];
            var identity = _redis.Hash[$"identity:{id}"];
            var pattern = await param.Get("password_format");
            if ((pattern != null && !Regex.IsMatch(password.ClearText, pattern)) ||
                (await param.Get("password_initial") == "Enable" && await param.Get("default_password") == password.ClearText))
            {
                return false;
            }
            var super = await Super(s => s.USER_ID == id);
            var timestamp = DateTime.Now;
            if (super != null)
            {
                super.USER_PASSWORD = password.CipherText;
                var json = JsonHelper.Serialize(super);
                await _database.From<T_SYS_PARAM>()
                    .Where(p => p.PARAM_KEY == "super_user")
                    .Update(new
                    {
                        PARAM_VALUE = json,
                        UPDATE_TIME = timestamp
                    });
                await param.Set("super_user", json);
            }
            else
            {
                await _database.From<T_SYS_USER>()
                    .Where(u => u.USER_ID == id)
                    .Update(new
                    {
                        USER_PASSWORD = password.CipherText,
                        USER_PASSWORD_TIME = timestamp,
                        UPDATE_TIME = timestamp
                    });
            }
            return true;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.ModifyAvatar(int id, string avatar) 方法
         */
        public async Task<bool> ModifyAvatar(int id, string avatar)
        {
            var super = await Super(s => s.USER_ID == id);
            if (super != null)
            {
                super.USER_AVATAR = avatar;
                var json = JsonHelper.Serialize(super);
                await _database.From<T_SYS_PARAM>()
                    .Where(p => p.PARAM_KEY == "super_user")
                    .Update(new
                    {
                        PARAM_VALUE = json,
                        UPDATE_TIME = DateTime.Now
                    });
                await _redis.Hash["param"].Set("super_user", json);
            }
            else
            {
                await _database.From<T_SYS_USER>()
                    .Where(u => u.USER_ID == id)
                    .Update(new
                    {
                        USER_AVATAR = avatar,
                        UPDATE_TIME = DateTime.Now
                    });
            }
            return true;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.IUserRepository.MenuPermission(int id) 方法
         */
        public async Task<IEnumerable<string>> MenuPermission(int id)
        {
            if (await Super(s => s.USER_ID == id) != null)
            {
                return null;
            }
            var menu = await _database.From<T_SYS_MENU, T_SYS_ROLE_MENU, T_SYS_USER_ROLE>()
                .Join<T_SYS_MENU, T_SYS_ROLE_MENU>(Join.Inner, (m, rm) => m.MENU_ID == rm.MENU_ID && m.MENU_FLAG == true && m.MENU_URL != null)
                .Join<T_SYS_ROLE_MENU, T_SYS_USER_ROLE>(Join.Inner, (rm, ur) => rm.ROLE_ID == ur.ROLE_ID && ur.USER_ID == id)
                .Distinct()
                .Select((m, _, _) => new { m.MENU_URL });
            var identity = _redis.Hash[$"identity:{id}"];
            var version = await identity.Get("url_version");
            if (version == null)
            {
                await identity.Set("url_version", version = "0");
            }
            var urls = menu.Select(m =>
            {
                string url = HttpUtility.UrlEncode(m.MENU_URL);
                return url;
            });
            if (urls.Any())
            {
                await _redis.Batch(batch => batch.Execute(urls.Select(u => identity.Set($"url_{u}", version)).ToArray()));
            }
            return urls;
        }

        #endregion

        #region Kean.Domain.Basic.Repositories.IUserRepository 接口实现部分

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.IsExist(int id) 方法
         */
        public async Task<bool> IsExist(int id)
        {
            return (await _database.From<T_SYS_USER>()
                .Where(r => r.USER_ID == id)
                .Single(r => new { Count = Function.Count(r.USER_ID) })).Count > 0;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.IsNameExist(string name, int? igrone) 方法
         */
        public async Task<bool> IsNameExist(string name, int? igrone)
        {
            var schema = _database.From<T_SYS_USER>().Where(r => r.USER_NAME == name);
            if (igrone.HasValue)
            {
                schema = schema.Where(r => r.USER_ID != igrone.Value);
            }
            return (await schema.Single(r => new { Count = Function.Count(r.USER_ID) })).Count > 0;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.IsAccountExist(string account, int? igrone) 方法
         */
        public async Task<bool> IsAccountExist(string account, int? igrone)
        {
            var schema = _database.From<T_SYS_USER>().Where(r => r.USER_ACCOUNT == account);
            if (igrone.HasValue)
            {
                schema = schema.Where(r => r.USER_ID != igrone.Value);
            }
            return (await schema.Single(r => new { Count = Function.Count(r.USER_ID) })).Count > 0;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.Create(User user, Func<string, string> encode) 方法
         */
        public async Task<int> Create(User user, Func<string, Task<string>> encode)
        {
            var timestamp = DateTime.Now;
            var entity = _mapper.Map<T_SYS_USER>(user);
            entity.USER_PASSWORD = await encode(await _redis.Hash["param"].Get("default_password"));
            entity.USER_PASSWORD_TIME = timestamp;
            entity.CREATE_TIME = timestamp;
            entity.UPDATE_TIME = timestamp;
            var id = await _database.From<T_SYS_USER>().Add(entity);
            if (id != null && user.Role != null)
            {
                foreach (var item in user.Role)
                {
                    if ((await _database.From<T_SYS_ROLE>()
                        .Where(r => r.ROLE_ID == item)
                        .Single(r => new { Count = Function.Count(r.ROLE_ID) }))
                        .Count > 0)
                    {
                        await _database.From<T_SYS_USER_ROLE>().Add(new()
                        {
                            USER_ID = Convert.ToInt32(id),
                            ROLE_ID = item,
                            CREATE_TIME = timestamp,
                            UPDATE_TIME = timestamp
                        });
                    }
                }
            }
            return Convert.ToInt32(id);
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.Modify(User user) 方法
         */
        public async Task Modify(User user)
        {
            var timestamp = DateTime.Now;
            await _database.From<T_SYS_USER>().Where(u => u.USER_ID == user.Id).Update(new
            {
                USER_NAME = user.Name,
                USER_ACCOUNT = user.Account,
                UPDATE_TIME = timestamp
            });
            await _database.From<T_SYS_USER_ROLE>().Where(r => r.USER_ID == user.Id).Delete();
            if (user.Role != null)
            {
                foreach (var item in user.Role)
                {
                    await _database.From<T_SYS_USER_ROLE>().Add(new()
                    {
                        USER_ID = user.Id,
                        ROLE_ID = item,
                        CREATE_TIME = timestamp,
                        UPDATE_TIME = timestamp
                    });
                }
            }
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.Delete(int id) 方法
         */
        public async Task Delete(int id)
        {
            await _database.From<T_SYS_USER>()
                .Where(u => u.USER_ID == id)
                .Delete();
            await _database.From<T_SYS_USER_ROLE>()
                .Where(r => r.USER_ID == id)
                .Delete();
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.ResetPassword(int id, Func<string, string> encode) 方法
         */
        public async Task ResetPassword(int id, Func<string, Task<string>> encode)
        {
            var timestamp = DateTime.Now;
            await _database.From<T_SYS_USER>()
                .Where(r => r.USER_ID == id)
                .Update(new
                {
                    USER_PASSWORD = await encode(await _redis.Hash["param"].Get("default_password")),
                    USER_PASSWORD_TIME = timestamp,
                    UPDATE_TIME = timestamp
                });
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IUserRepository.ClearSession(int id) 方法
         */
        public async Task ClearSession(int id)
        {
            var sessions = (await _redis.Hash[$"identity:{id}"].Range()).Where(i => i.Key.StartsWith("session_")).Select(i => i.Key[8..]);
            await _redis.Hash["identity:index"].Set(id.ToString(), null);
            await _redis.String[$"identity:{id}"].Set(null);
            if (sessions.Any())
            {
                await _redis.Batch(batch => batch.Execute(sessions.Select(s => batch.String[$"session:{s}"].Set(null)).ToArray()));
            }
        }

        #endregion
    }
}
