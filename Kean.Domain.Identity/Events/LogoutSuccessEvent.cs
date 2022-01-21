namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 注销成功时触发的事件
    /// </summary>
    public class LogoutSuccessEvent : IEvent
    {
        /// <summary>
        /// 会话
        /// </summary>
        public string Session { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
}
