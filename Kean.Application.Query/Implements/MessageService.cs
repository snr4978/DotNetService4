using AutoMapper;
using Kean.Application.Query.Interfaces;
using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Database;
using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Query.Implements
{
    /// <summary>
    /// 消息查询服务实现
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultDb _database; // 默认数据库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MessageService(
            IMapper mapper,
            IDefaultDb database)
        {
            _mapper = mapper;
            _database = database;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IMessageService.GetCount(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag) 方法
         */
        public async Task<int> GetCount(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag)
        {
            int? sourceId = null;
            if (!string.IsNullOrWhiteSpace(source))
            {
                sourceId = (await _database.From<T_SYS_USER>().Where(u => u.USER_NAME == source).Single(u => new { u.USER_ID }))?.USER_ID;
                if (!sourceId.HasValue)
                {
                    return 0;
                }
            }
            var schema = _database.From<T_SYS_USER_MESSAGE>($"T_SYS_USER_MESSAGE_{userId}");
            if (!string.IsNullOrWhiteSpace(subject))
            {
                schema = schema.Where(m => m.MESSAGE_SUBJECT.Contains(subject));
            }
            if (sourceId.HasValue)
            {
                schema = schema.Where(m => m.MESSAGE_SOURCE == sourceId.Value);
            }
            if (start.HasValue)
            {
                schema = schema.Where(m => m.MESSAGE_TIME >= start.Value);
            }
            if (end.HasValue)
            {
                schema = schema.Where(m => m.MESSAGE_TIME <= end.Value.AddDays(1));
            }
            if (flag.HasValue)
            {
                schema = schema.Where(m => m.MESSAGE_FLAG == flag.Value);
            }
            return (await schema.Single(m => new { Count = Function.Count(m.MESSAGE_ID) })).Count;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IMessageService.GetList(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag, int? offset, int? limit) 方法
         */
        public async Task<IEnumerable<Message>> GetList(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag, int? offset, int? limit)
        {
            var schema = _database.From<T_SYS_USER_MESSAGE, T_SYS_USER>(name1: $"T_SYS_USER_MESSAGE_{userId}")
                .Join(Join.Left, (m, u) => m.MESSAGE_SOURCE == u.USER_ID)
                .OrderBy((m, u) => m.MESSAGE_TIME, Order.Descending);
            if (!string.IsNullOrWhiteSpace(subject))
            {
                schema = schema.Where((m, _) => m.MESSAGE_SUBJECT.Contains(subject));
            }
            if (!string.IsNullOrWhiteSpace(source))
            {
                schema = schema.Where((_, u) => u.USER_NAME == source);
            }
            if (start.HasValue)
            {
                schema = schema.Where((m, _) => m.MESSAGE_TIME >= start.Value);
            }
            if (end.HasValue)
            {
                schema = schema.Where((m, _) => m.MESSAGE_TIME <= end.Value.AddDays(1));
            }
            if (flag.HasValue)
            {
                schema = schema.Where((m, _) => m.MESSAGE_FLAG == flag.Value);
            }
            if (offset.HasValue)
            {
                schema = schema.Skip(offset.Value);
            }
            if (limit.HasValue)
            {
                schema = schema.Take(limit.Value);
            }
            return _mapper.Map<IEnumerable<Message>>(await schema.Select((m, u) => new
            {
                m.MESSAGE_ID,
                m.MESSAGE_TIME,
                m.MESSAGE_SUBJECT,
                m.MESSAGE_CONTENT,
                m.MESSAGE_FLAG,
                u.USER_ID,
                u.USER_NAME,
                u.USER_AVATAR
            }));
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IMessageService.GetItem(int userId, int messageId) 方法
         */
        public async Task<Message> GetItem(int userId, int messageId)
        {
            return _mapper.Map<Message>(await _database.From<T_SYS_USER_MESSAGE, T_SYS_USER>(name1: $"T_SYS_USER_MESSAGE_{userId}")
                .Join(Join.Left, (m, u) => m.MESSAGE_SOURCE == u.USER_ID)
                .Where((m, u) => m.MESSAGE_ID == messageId)
                .Single((m, u) => new
                {
                    m.MESSAGE_ID,
                    m.MESSAGE_TIME,
                    m.MESSAGE_SUBJECT,
                    m.MESSAGE_CONTENT,
                    m.MESSAGE_FLAG,
                    u.USER_ID,
                    u.USER_NAME,
                    u.USER_AVATAR
                }));
        }
    }
}
