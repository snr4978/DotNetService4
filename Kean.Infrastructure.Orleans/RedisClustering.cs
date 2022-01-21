namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// Redis 信息
    /// </summary>
    public sealed class RedisClustering
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379";

        /// <summary>
        /// DB
        /// </summary>
        public int Database { get; set; } = 1;
    }
}
