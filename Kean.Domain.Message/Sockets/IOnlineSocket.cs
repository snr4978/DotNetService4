using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Message.Sockets
{
    /// <summary>
    /// 在线管理通道
    /// </summary>
    public interface IOnlineSocket
    {
        /// <summary>
        /// 消息通知
        /// </summary>
        /// <param name="connectionIds">连接 ID</param>
        Task Notify(IEnumerable<string> connectionIds);
    }
}
