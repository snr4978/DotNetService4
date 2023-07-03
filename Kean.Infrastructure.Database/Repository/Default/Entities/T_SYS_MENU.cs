using System;

namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_MENU : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Identifier]
        public int MENU_ID { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public int MENU_PARENT_ID { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string MENU_TYPE { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MENU_NAME { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string MENU_PARAM { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string MENU_ICON { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public bool MENU_FLAG { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int MENU_ORDER { get; set; }

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
