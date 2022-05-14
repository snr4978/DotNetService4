using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 消息仓库
    /// </summary>
    public class MessageRepository :
        Domain.Message.Repositories.IMessageRepository
    {
        private readonly IDefaultDb _database; // 默认数据库
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MessageRepository(
            IDefaultDb database,
            IDefaultRedis redis)
        {
            _database = database;
            _redis = redis;
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.SendMessage(string subject, string content, int source, int target, DateTime time) 方法
         */
        public Task<bool> SendMessage(string subject, string content, int source, int target, DateTime time)
        {
            return _database.From<T_SYS_USER_MESSAGE>($"T_SYS_USER_MESSAGE_{target}")
                .Add(new()
                {
                    MESSAGE_TIME = time,
                    MESSAGE_SOURCE = source,
                    MESSAGE_SUBJECT = subject,
                    MESSAGE_CONTENT = content,
                    MESSAGE_FLAG = false
                })
                .ContinueWith(r => !r.IsFaulted && r.Result != null);
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.MarkMessage(int userId, int messageId, bool flag) 方法
         */
        public Task MarkMessage(int userId, int messageId, bool flag)
        {
            return _database.From<T_SYS_USER_MESSAGE>($"T_SYS_USER_MESSAGE_{userId}")
                .Where(m => m.MESSAGE_ID == messageId)
                .Update(new { MESSAGE_FLAG = flag });
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.DeleteMessage(int userId, int messageId) 方法
         */
        public Task DeleteMessage(int userId, int messageId)
        {
            return _database.From<T_SYS_USER_MESSAGE>($"T_SYS_USER_MESSAGE_{userId}")
                .Where(m => m.MESSAGE_ID == messageId)
                .Delete();
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.GetConnections(int userId) 方法
         */
        public async Task<IEnumerable<string>> GetConnections(int userId)
        {
            var sessions = (await _redis.Hash[$"identity:{userId}"].Range())
                .Where(i => i.Key.StartsWith("session:"));
            return sessions.Any() ?
                await _redis.Batch(batch => batch.Execute(sessions.Select(s => batch.Hash[$"session:{s.Key[8..]}"].Get("message")).ToArray())) :
                Array.Empty<string>();
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.RegisterConnection(string session, string id) 方法
         */
        public async Task<bool> RegisterConnection(string session, string id)
        {
            var hash = _redis.Hash[$"session:{session}"];
            if (await hash.Get("identity") == null)
            {
                return false;
            }
            else
            {
                await hash.Set("message", id);
                return true;
            }
        }

        /*
         * 实现 Kean.Domain.Message.Repositories.IMessageRepository.UnregisterConnection(string session, string id) 方法
         */
        public async Task<bool> UnregisterConnection(string session, string id)
        {
            var hash = _redis.Hash[$"session:{session}"];
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
