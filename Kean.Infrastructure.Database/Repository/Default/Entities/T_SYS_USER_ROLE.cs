using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_USER_ROLE : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public int REL_ID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public int USER_ID { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int ROLE_ID { get; set; }

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
