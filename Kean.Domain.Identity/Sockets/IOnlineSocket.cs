using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.Sockets
{
    /// <summary>
    /// 在线管理通道
    /// </summary>
    public interface IOnlineSocket
    {
        /// <summary>
        /// 通知下线
        /// </summary>
        /// <param name="connectionIds">连接 ID</param>
        Task Offline(IEnumerable<string> connectionIds);
    }
}
