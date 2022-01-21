namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// Sql 查询
    /// </summary>
    public sealed class Query
    {
        /// <summary>
        /// Sql 查询命令
        /// </summary>
        internal string Expression { get; set; }

        /// <summary>
        /// 命令参数
        /// </summary>
        internal Parameters Parameters { get; set; }

        /// <summary>
        /// 查询结果集是否包含项目
        /// </summary>
        public bool Contains(object item) => default;
    }
}
