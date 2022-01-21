using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kean.Application.Command.ViewModels
{
    /// <summary>
    /// 用户视图
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
        /// 密码
        /// </summary>
        [JsonConverter(typeof(Password))] 
        public Password Password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [JsonConverter(typeof(Avatar))]
        public Avatar Avatar { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<int> Role { get; set; }
    }
}
