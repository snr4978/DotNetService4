namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 基于 StackExchange.Redis 的驱动
    /// </summary>
    internal sealed class StackExchangeDriver : IDriver
    {
        private readonly string _connectionString;
        private readonly int _database;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">缓存连接字符串</param>
        /// <param name="database">DB 索引</param>
        public StackExchangeDriver(string connectionString, int database)
        {
            _connectionString = connectionString;
            _database = database;
        }

        /*
         * 实现 Kean.Infrastructure.NoSql.Redis.IDriver.CreateContext ，表示创建缓存连接
         */
        public IContext CreateContext()
        {
            return new StackExchangeContext(_connectionString, _database);
        }
    }
}
