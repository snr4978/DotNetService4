using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_PARAM : IEntity
    {
        /// <summary>
        /// 参数名
        /// </summary>
        [Identifier]
        public string PARAM_KEY { get; set; }

        /// <summary>
		/// 参数值
        /// </summary>
        public string PARAM_VALUE { get; set; }

        /// <summary>
		/// 值规则
        /// </summary>
        public string PARAM_VALID { get; set; }

        /// <summary>
		/// 备注
        /// </summary>
        public string PARAM_REMARK { get; set; }

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
