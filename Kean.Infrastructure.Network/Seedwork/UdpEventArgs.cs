using System;
using System.Net;

namespace Kean.Infrastructure.Network
{
    /// <summary>
    /// 为 UDP 套接字操作事件提供数据
    /// </summary>
    public class UdpEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置终结点
        /// </summary>
        public EndPoint EndPoint { get; set; }

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
