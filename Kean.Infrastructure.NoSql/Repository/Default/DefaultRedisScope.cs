using Kean.Infrastructure.NoSql.Redis;

namespace Kean.Infrastructure.NoSql.Repository.Default
{
    /// <summary>
    /// 默认 Redis 数据域
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="V">数据值类型</typeparam>
    public class DefaultRedisScope<T, V> where T : IValue<V>
    {
        private const string NAME = "";

        private readonly T _type;

        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Repository.Default.DefaultRedisScope 类的新实例
        /// </summary>
        /// <param name="type">数据实例</param>
        internal DefaultRedisScope(T type) => 
            _type = type;

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public V this[string key] => 
            _type[$"{NAME}{key}"];
    }
}
