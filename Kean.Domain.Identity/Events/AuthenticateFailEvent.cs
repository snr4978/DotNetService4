namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 身份验证失败时触发的事件
    /// </summary>
    public class AuthenticateFailEvent : IEvent
    {
        /// <summary>
        /// 会话
        /// </summary>
        public string Session { get; set; }
    }
}
