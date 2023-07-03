namespace Kean.Domain.Admin.Events
{
    /// <summary>
    /// 创建角色成功时触发的事件
    /// </summary>
    public class CreateRoleSuccessEvent : IEvent
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
