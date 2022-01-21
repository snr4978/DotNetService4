namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 初始化密码成功时触发的事件
    /// </summary>
    public class InitializePasswordSuccessEvent : IEvent
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id { get; set; }
    }
}
