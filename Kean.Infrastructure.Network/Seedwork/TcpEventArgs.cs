using System;
using System.Net.Sockets;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// 为 TCP 套接字操作事件提供数据
    /// </summary>
    public class TcpEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置套接字
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 获取或设置数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 获取或设置异常
        /// </summary>
        public Exception Exception { get; set; }
    }
}
