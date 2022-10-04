using System.Collections.Generic;

namespace Kean.Domain.Admin.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 获取或设置标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 获取或设置角色
        /// </summary>
        public IEnumerable<int> Role { get; set; }
    }
}
