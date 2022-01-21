using System.Collections.Generic;

namespace Kean.Application.Query.ViewModels
{
    /// <summary>
    /// 用户信息视图
    /// </summary>
    public sealed class User
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
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<Role> Role { get; set; }
    }
}
