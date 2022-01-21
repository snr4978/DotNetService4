using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_SECURITY_LOG : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public virtual string LOG_ID { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual string LOG_TAG { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime LOG_TIME { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string LOG_CONTENT { get; set; }
    }
}
