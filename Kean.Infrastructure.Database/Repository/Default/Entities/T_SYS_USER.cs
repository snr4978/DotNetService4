using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_USER : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public int USER_ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string USER_NAME { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string USER_ACCOUNT { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string USER_PASSWORD { get; set; }

        /// <summary>
        /// 密码时间戳
        /// </summary>
        public DateTime? USER_PASSWORD_TIME { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string USER_AVATAR { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CREATE_TIME { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UPDATE_TIME { get; set; }
    }
}
