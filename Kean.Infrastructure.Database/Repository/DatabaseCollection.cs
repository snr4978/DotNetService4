using System.Collections.Generic;

namespace Kean.Infrastructure.Database.Repository
{
    /// <summary>
    /// 数据库连接集合
    /// </summary>
    public sealed class DatabaseCollection : List<IDatabase>, IDatabaseCollection
    {
    }
}
