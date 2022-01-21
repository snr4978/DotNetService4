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
        /// 标题
        /// </summary>
        public string MENU_HEADER { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string MENU_URL { get; set; }

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
    }
}
