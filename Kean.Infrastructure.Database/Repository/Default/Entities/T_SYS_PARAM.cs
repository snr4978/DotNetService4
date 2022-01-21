namespace Kean.Infrastructure.Database.Repository.Default.Entities
{
    public class T_SYS_PARAM : IEntity
    {
        /// <summary>
        /// 参数名
        /// </summary>
        [Identifier]
        public virtual string PARAM_KEY { get; set; }

        /// <summary>
		/// 参数值
        /// </summary>
        public virtual string PARAM_VALUE { get; set; }

        /// <summary>
		/// 备注
        /// </summary>
        public virtual string PARAM_REMARK { get; set; }
    }
}
