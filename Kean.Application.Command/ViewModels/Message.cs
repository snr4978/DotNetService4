namespace Kean.Application.Command.ViewModels
{
    /// <summary>
    /// 消息视图
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息源（发送者的用户 ID）
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public bool Flag { get; set; }
    }
}
