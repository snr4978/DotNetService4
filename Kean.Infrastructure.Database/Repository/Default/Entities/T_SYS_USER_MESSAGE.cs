using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_USER_MESSAGE : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public int MESSAGE_ID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime MESSAGE_TIME { get; set; }

        /// <summary>
        /// 消息源
        /// </summary>
        public int MESSAGE_SOURCE { get; set; }

        /// <summary>
        /// 消息目标
        /// </summary>
        public int MESSAGE_TARGET { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string MESSAGE_SUBJECT { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string MESSAGE_CONTENT { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public bool MESSAGE_FLAG { get; set; }

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
