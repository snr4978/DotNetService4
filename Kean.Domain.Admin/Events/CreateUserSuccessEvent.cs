using System.Collections.Generic;

namespace Kean.Domain.Admin.Events
{
    /// <summary>
    /// 创建用户成功时触发的事件
    /// </summary>
    public class CreateUserSuccessEvent : IEvent
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
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<int> Role { get; set; }
    }
}
