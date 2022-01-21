namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表示数据库驱动
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        IDbContext CreateContext();
    }
}
