using System;

namespace Kean.Application.Query.ViewModels
{
    /// <summary>
    /// 消息信息视图
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 消息源
        /// </summary>
        public User Source { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public bool Flag { get; set; }
    }
}
