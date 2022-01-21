namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 身份验证成功时触发的事件
    /// </summary>
    public class AuthenticateSuccessEvent : IEvent
    {
        /// <summary>
        /// 会话
        /// </summary>
        public string Session { get; set; }
    }
}
