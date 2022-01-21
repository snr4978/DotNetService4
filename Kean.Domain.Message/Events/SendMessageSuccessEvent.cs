using System.Collections.Generic;

namespace Kean.Domain.Message.Events
{
    /// <summary>
    /// 发送消息成功时触发的事件
    /// </summary>
    public class SendMessageSuccessEvent : IEvent
    {
        /// <summary>
        /// 目标（用户 ID 集合）
        /// </summary>
        public IEnumerable<int> Targets { get; set; }
    }
}
