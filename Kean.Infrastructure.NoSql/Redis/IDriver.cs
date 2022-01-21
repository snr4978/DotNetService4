namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 表示 Redis 驱动
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// 创建 Redis 连接
        /// </summary>
        /// <returns>Redis 连接</returns>
        IContext CreateContext();
    }
}
