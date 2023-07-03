using System.Collections.Generic;

namespace Kean.Domain.Admin.Events
{
    /// <summary>
    /// 设置角色菜单权限成功时触发的事件
    /// </summary>
    public class SetMenuPermissionSuccessEvent : IEvent
    {
        /// <summary>
        /// 角色 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public IEnumerable<int> Permission { get; set; }
    }
}
