using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 表示 Redis 批量操作上下文
    /// </summary>
    public interface IBatch
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
        /// 执行批次命令
        /// </summary>
        /// <param name="tasks">任务</param>
        Task Execute(params Task[] tasks);

        /// <summary>
        /// 执行批次命令
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="tasks">任务</param>
        /// <returns>返回值</returns>
        Task<T[]> Execute<T>(params Task<T>[] tasks);
    }
}
