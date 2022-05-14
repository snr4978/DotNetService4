using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_SECURITY_LOG : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public string LOG_ID { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string LOG_TAG { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime LOG_TIME { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string LOG_CONTENT { get; set; }

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
