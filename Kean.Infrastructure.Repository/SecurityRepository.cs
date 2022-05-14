using Kean.Infrastructure.Database.Repository.Default;
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
    /// 安全仓库
    /// </summary>
    public class SecurityRepository :
        Domain.App.Repositories.ISecurityRepository,
        Domain.Identity.Repositories.ISecurityRepository
    {
        private readonly IDefaultDb _database; // 默认数据库
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SecurityRepository(
            IDefaultDb database,
            IDefaultRedis redis)
        {
            _database = database;
            _redis = redis;
        }

        /*
         * 实现 Kean.Domain.App.Repositories.ISecurityRepository.LoadBlacklist() 方法
         */
        public async Task LoadBlacklist()
        {
            var blacklist = await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Address" && s.SECURITY_STATUS == 0).Select();
            if (blacklist.Any())
            {
                await _redis.Batch(batch =>
                {
                    var hash = batch.Hash["blacklist"];
                    return batch.Execute(blacklist.Select(b => hash.Set(b.SECURITY_VALUE, JsonHelper.Serialize(b))).ToArray());
                });
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.SignAddress(string address) 方法
         */
        public async Task SignAddress(string address)
        {
            if (int.TryParse(await _redis.Hash["param"].Get("address_security"), out int limit) && limit > 0)
            {
                var security = await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Address" && s.SECURITY_VALUE == address).Single();
                if (security == null)
                {
                    await _database.From<T_SYS_SECURITY>().Add(new()
                    {
                        SECURITY_TYPE = "Address",
                        SECURITY_VALUE = address,
                        SECURITY_STATUS = 1,
                        SECURITY_TIMESTAMP = DateTime.Now
                    });
                }
                else if (security.SECURITY_STATUS > 0)
                {
                    security.SECURITY_TIMESTAMP = DateTime.Now;
                    if (++security.SECURITY_STATUS > limit)
                    {
                        security.SECURITY_STATUS = 0;
                        await _redis.Hash["blacklist"].Set(address, JsonHelper.Serialize(security));
                    }
                    await _database.From<T_SYS_SECURITY>().Update(security);
                }
                else
                {
                    await _redis.Hash["blacklist"].Set(address, JsonHelper.Serialize(security));
                }
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.UnsignAddress(string address) 方法
         */
        public async Task UnsignAddress(string address)
        {
            await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Address" && s.SECURITY_VALUE == address).Delete();
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.SignAccount(string account) 方法
         */
        public async Task SignAccount(string account)
        {
            if (int.TryParse(await _redis.Hash["param"].Get("account_security"), out int limit) && limit > 0)
            {
                var security = await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Account" && s.SECURITY_VALUE == account).Single();
                if (security == null)
                {
                    await _database.From<T_SYS_SECURITY>().Add(new()
                    {
                        SECURITY_TYPE = "Account",
                        SECURITY_VALUE = account,
                        SECURITY_STATUS = 1,
                        SECURITY_TIMESTAMP = DateTime.Now
                    });
                }
                else if (security.SECURITY_STATUS > 0)
                {
                    security.SECURITY_TIMESTAMP = DateTime.Now;
                    if (++security.SECURITY_STATUS > limit)
                    {
                        security.SECURITY_STATUS = 0;
                    }
                    await _database.From<T_SYS_SECURITY>().Update(security);
                }
            }
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.UnsignAccount(string account) 方法
         */
        public async Task UnsignAccount(string account)
        {
            await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Account" && s.SECURITY_VALUE == account).Delete();
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.AccountIsFrozen(string account) 方法
         */
        public async Task<bool> AccountIsFrozen(string account)
        {
            return await _database.From<T_SYS_SECURITY>().Where(s => s.SECURITY_TYPE == "Account" && s.SECURITY_VALUE == account && s.SECURITY_STATUS == 0).Single() != null;
        }

        /*
         * 实现 Kean.Domain.Identity.Repositories.ISecurityRepository.WriteLog(string tag, string content) 方法
         */
        public async Task WriteLog(string tag, string content)
        {
            if (await _redis.Hash["param"].Get("security_log") == "Enable")
            {
                await _database.From<T_SYS_SECURITY_LOG>().Add(new()
                {
                    LOG_TAG = tag,
                    LOG_TIME = DateTime.Now,
                    LOG_CONTENT = content
                });
            }
        }
    }
}
