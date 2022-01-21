using System.Collections.Generic;

namespace Kean.Infrastructure.Database.Repository
{
    /// <summary>
    /// 表示数据库连接集合
    /// </summary>
    public interface IDatabaseCollection : IList<IDatabase>
    {
    }
}
