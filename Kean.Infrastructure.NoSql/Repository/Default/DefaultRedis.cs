using Kean.Infrastructure.NoSql.Redis;
using System;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Repository.Default
{
    using String = Redis.String;

    /// <summary>
    /// 默认 Redis
    /// </summary>
    public sealed class DefaultRedis : IDefaultRedis
    {
        private IContext _context; 
        private DefaultRedisScope<String, String.Value> _string;
        private DefaultRedisScope<Hash, Hash.Value> _hash;
        private DefaultRedisScope<List, List.Value> _list;
        private DefaultRedisScope<Set, Set.Value> _set;
        private DefaultRedisScope<Zset, Zset.Value> _zset;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultRedis() =>
            _context = Configuration.Configure("Default").CreateContext();

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.String
         */
        public DefaultRedisScope<String, String.Value> String => 
            _string ??= new(_context.String);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.Hash
         */
        public DefaultRedisScope<Hash, Hash.Value> Hash => 
            _hash ??= new(_context.Hash);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.List
         */
        public DefaultRedisScope<List, List.Value> List => 
            _list ??= new(_context.List);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.Set
         */
        public DefaultRedisScope<Set, Set.Value> Set => 
            _set ??= new(_context.Set);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.Zset
         */
        public DefaultRedisScope<Zset, Zset.Value> Zset => 
            _zset ??= new(_context.Zset);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.Batch(Func<IBatch, Task> task)
         */
        public Task Batch(Func<IBatch, Task> task) => 
            _context.Batch(task);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Repository.Default.IDefaultRedis.Batch<T>(Func<IBatch, Task<IEnumerable<T>>> task)
         */
        public Task<T[]> Batch<T>(Func<IBatch, Task<T[]>> task) =>
            _context.Batch(task);

        /*
         * 实现 System.IDisposable.Dispose
         */
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}
