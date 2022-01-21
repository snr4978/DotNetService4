namespace Kean.Infrastructure.Database.Repository.Default
{
    /// <summary>
    /// 默认数据库连接
    /// </summary>
    public sealed class DefaultDb : AbstractDatabase, IDefaultDb
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultDb(IDatabaseCollection databaseCollection) 
            : base(databaseCollection, "Default")
        {
        }
    }
}
