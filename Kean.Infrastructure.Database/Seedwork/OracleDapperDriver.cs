namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 基于 Dapper 的 Oracle 数据库驱动
    /// </summary>
    internal sealed class OracleDapperDriver : IDriver
    {
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public OracleDapperDriver(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public IDbContext CreateContext()
        {
            IDbContext context = new OracleDapperContext(_connectionString);
            context.Connection.Open();
            return context;
        }
    }
}
