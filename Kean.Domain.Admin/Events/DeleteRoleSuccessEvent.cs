using System.Collections.Generic;

namespace Kean.Domain.Admin.Events
{
    /// <summary>
    /// 删除角色成功时触发的事件
    /// </summary>
    public class DeleteRoleSuccessEvent : IEvent
    {
        /// <summary>
        /// 标识
        /// </summary>
        public IEnumerable<int> Id { get; set; }
    }
}
