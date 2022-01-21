using Kean.Infrastructure.NoSql.Redis;
using System;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Repository.Default
{
    using String = Redis.String;

    /// <summary>
    /// 表示默认 Redis
    /// </summary>
    public interface IDefaultRedis
    {
        /// <summary>
        /// 访问字符串类型
        /// </summary>
        DefaultRedisScope<String, String.Value> String { get; }

        /// <summary>
        /// 访问哈希类型
        /// </summary>
        DefaultRedisScope<Hash, Hash.Value> Hash { get; }

        /// <summary>
        /// 访问列表类型
        /// </summary>
        DefaultRedisScope<List, List.Value> List { get; }

        /// <summary>
        /// 访问集合类型
        /// </summary>
        DefaultRedisScope<Set, Set.Value> Set { get; }

        /// <summary>
        /// 访问有序集合类型
        /// </summary>
        DefaultRedisScope<Zset, Zset.Value> Zset { get; }

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
