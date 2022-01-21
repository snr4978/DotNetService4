namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// Redis 仓库
    /// </summary>
    public sealed class RedisStorage
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379";

        /// <summary>
        /// DB
        /// </summary>
        public int Database { get; set; } = 2;
    }
}
