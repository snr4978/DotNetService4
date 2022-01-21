using System.Collections.Generic;

namespace Kean.Domain.Basic.Events
{
    /// <summary>
    /// 删除用户成功时触发的事件
    /// </summary>
    public class DeleteUserSuccessEvent : IEvent
    {
        /// <summary>
        /// 标识
        /// </summary>
        public IEnumerable<int> Id { get; set; }
    }
}
