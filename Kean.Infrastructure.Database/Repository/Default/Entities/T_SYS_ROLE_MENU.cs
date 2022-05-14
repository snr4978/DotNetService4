namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_ROLE_MENU : IEntity
    {
        /// <summary>
        /// 自增主键
        /// </summary>
        [Identifier(true)]
        public int REL_ID { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int ROLE_ID { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public int MENU_ID { get; set; }
    }
}
