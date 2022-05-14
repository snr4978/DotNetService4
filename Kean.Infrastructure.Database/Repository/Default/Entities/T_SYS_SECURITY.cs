using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_SECURITY : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public virtual string SECURITY_ID { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string SECURITY_TYPE { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual string SECURITY_VALUE { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int SECURITY_STATUS { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public virtual DateTime SECURITY_TIMESTAMP { get; set; }
    }
}
