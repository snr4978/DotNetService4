using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Message.Repositories
{
    /// <summary>
    /// 表示消息仓库
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        /// <param name="time">时间</param>
        /// <returns>发送结果</returns>
        Task<bool> SendMessage(string subject, string content, int source, int target, DateTime time);

        /// <summary>
        /// 标记消息
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="messageId">消息 ID</param>
        /// <param name="flag">状态标记：如果标记为已读，为 true，否则为 false</param>
        Task MarkMessage(int userId, int messageId, bool flag);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="messageId">消息 ID</param>
        Task DeleteMessage(int userId, int messageId);

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <returns>连接 ID</returns>
        Task<IEnumerable<string>> GetConnections(int userId);

        /// <summary>
        /// 注册连接
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="id">连接码</param>
        /// <returns>如果注册成功，为 true；否则为 false</returns>
        Task<bool> RegisterConnection(string session, string id);

        /// <summary>
        /// 注销连接
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="id">连接码</param>
        /// <returns>如果注销成功，为 true；否则为 false</returns>
        Task<bool> UnregisterConnection(string session, string id);
    }
}
