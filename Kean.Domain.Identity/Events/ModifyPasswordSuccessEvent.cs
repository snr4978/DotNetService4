namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 修改密码成功时触发的事件
    /// </summary>
    public class ModifyPasswordSuccessEvent : IEvent
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id { get; set; }
    }
}
