using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_ROLE : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public int ROLE_ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ROLE_NAME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ROLE_REMARK { get; set; }

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
