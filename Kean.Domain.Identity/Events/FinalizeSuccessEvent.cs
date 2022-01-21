using System.Collections.Generic;

namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 终结成功时触发的事件
    /// </summary>
    public class FinalizeSuccessEvent : IEvent
    {
        /// <summary>
        /// 会话
        /// </summary>
        public IEnumerable<string> Session { get; set; }
    }
}
