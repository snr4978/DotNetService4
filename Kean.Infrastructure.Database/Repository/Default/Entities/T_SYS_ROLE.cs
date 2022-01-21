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
    }
}
