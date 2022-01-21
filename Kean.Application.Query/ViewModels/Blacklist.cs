using System;

namespace Kean.Application.Query.ViewModels
{
    /// <summary>
    /// 黑名单信息视图
    /// </summary>
    public sealed class Blacklist
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
