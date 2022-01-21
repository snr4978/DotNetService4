using System;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 表示 Redis 操作上下文
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// 访问字符串类型
        /// </summary>
        String String { get; }

        /// <summary>
        /// 访问哈希类型
        /// </summary>
        Hash Hash { get; }

        /// <summary>
        /// 访问列表类型
        /// </summary>
        List List { get; }

        /// <summary>
        /// 访问集合类型
        /// </summary>
        Set Set { get; }

        /// <summary>
        /// 访问有序集合类型
        /// </summary>
        Zset Zset { get; }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="task">批量任务</param>
        Task Batch(Func<IBatch, Task> task);

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="task">批量任务</param>
        Task<T[]> Batch<T>(Func<IBatch, Task<T[]>> task);
    }
}
