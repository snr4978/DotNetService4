namespace Kean.Domain.Admin.Events
{
    /// <summary>
    /// 重置密码成功时触发的事件
    /// </summary>
    public class ResetPasswordSuccessEvent : IEvent
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }
    }
}
