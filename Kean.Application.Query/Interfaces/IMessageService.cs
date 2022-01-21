using Kean.Application.Query.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Query.Interfaces
{
    /// <summary>
    /// 表示消息查询服务
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 获取用户消息数
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="subject">主题</param>
        /// <param name="source">源</param>
        /// <param name="start">最早</param>
        /// <param name="end">最晚</param>
        /// <param name="flag">消息状态</param>
        /// <returns>消息数</returns>
        Task<int> GetCount(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag);

        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="subject">主题</param>
        /// <param name="source">源</param>
        /// <param name="start">最早</param>
        /// <param name="end">最晚</param>
        /// <param name="flag">消息状态</param>
        /// <param name="offset">偏移</param>
        /// <param name="limit">限制</param>
        /// <returns>结果视图</returns>
        Task<IEnumerable<Message>> GetList(int userId, string subject, string source, DateTime? start, DateTime? end, bool? flag, int? offset, int? limit);

        /// <summary>
        /// 获取用户消息内容
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="messageId">消息 ID</param>
        /// <returns>结果视图</returns>
        Task<Message> GetItem(int userId, int messageId);
    }
}
