using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_SECURITY : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public string SECURITY_ID { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string SECURITY_TYPE { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string SECURITY_VALUE { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int SECURITY_STATUS { get; set; }

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
