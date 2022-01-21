using Kean.Application.Command.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Command.Interfaces
{
    /// <summary>
    /// 表示消息服务
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="id">连接 ID</param>
        Task Connect(string token, string id);

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="id">连接 ID</param>
        Task Disconnect(string token, string id);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="targets">目标</param>
        /// <returns>发送结果</returns>
        Task<bool> SendMessage(Message message, params int[] targets);

        /// <summary>
        /// 标记消息
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="messageId">消息 ID</param>
        /// <param name="flag">标记：如果标记为已读，为 true，否则为 false</param>
        /// <returns>成功标记的 ID</returns>
        Task<IEnumerable<int>> MarkMessage(int userId, IEnumerable<int> messageId, bool flag);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <param name="messageId">消息 ID</param>
        /// <returns>成功删除的 ID</returns>
        Task<IEnumerable<int>> DeleteMessage(int userId, IEnumerable<int> messageId);
    }
}
