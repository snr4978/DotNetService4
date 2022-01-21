using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 基于 StackExchange.Redis 的操作上下文
    /// </summary>
    public class StackExchangeContext : IContext, IBatch
    {
        private readonly IDatabaseAsync _db;
        private readonly Lazy<String> _string;
        private readonly Lazy<Hash> _hash;
        private readonly Lazy<List> _list;
        private readonly Lazy<Set> _set;
        private readonly Lazy<Zset> _zset;
        private readonly bool _connection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="database">DB 索引</param>
        public StackExchangeContext(string connectionString, int database)
            : this(ConnectionMultiplexer.Connect(connectionString).GetDatabase(database))
        {
            _connection = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private StackExchangeContext(IDatabaseAsync db)
        {
            _db = db;
            _string = new(() =>
            {
                var @string = new String();
                @string.OnGet += String_OnGet;
                @string.OnSet += String_OnSet;
                return @string;
            });
            _hash = new(() =>
            {
                var hash = new Hash();
                hash.OnRange += Hash_OnRange;
                hash.OnGet += Hash_OnGet;
                hash.OnSet += Hash_OnSet;
                return hash;
            });
            _list = new(() =>
            {
                var list = new List();
                list.OnRange += List_OnRange;
                list.OnPopLeft += List_OnPopLeft;
                list.OnPopRight += List_OnPopRight;
                list.OnPushLeft += List_OnPushLeft;
                list.OnPushRight += List_OnPushRight;
                return list;
            });
            _set = new(() =>
            {
                var set = new Set();
                set.OnRange += Set_OnRange;
                set.OnAdd += Set_OnAdd;
                set.OnRemove += Set_OnRemove;
                return set;
            });
            _zset = new(() =>
            {
                var zset = new Zset();
                zset.OnRange += Zset_OnRange;
                zset.OnAdd += Zset_OnAdd;
                zset.OnRemove += Zset_OnRemove;
                return zset;
            });
        }

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.String
         */
        public String String { get => _string.Value; }

        /*
         * 获取字符串值
         */
        private Task<string> String_OnGet(string key) =>
            _db.StringGetAsync(key).ContinueWith<string>(t => t.Result);

        /*
         * 设置字符串值
         */
        private Task String_OnSet(string key, string value) =>
            value == null ? _db.KeyDeleteAsync(key) : _db.StringSetAsync(key, value);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.Hash
         */
        public Hash Hash { get => _hash.Value; }

        /*
         * 遍历哈希值
         */
        private Task<IDictionary<string, string>> Hash_OnRange(string key) =>
            _db.HashGetAllAsync(key).ContinueWith<IDictionary<string, string>>(t => t.Result.ToStringDictionary());

        /*
         * 获取哈希值
         */
        private Task<string> Hash_OnGet(string key, string field) =>
            _db.HashGetAsync(key, field).ContinueWith<string>(t => t.Result);

        /*
         * 设置哈希值
         */
        private Task Hash_OnSet(string key, string field, string value) =>
            value == null ? _db.HashDeleteAsync(key, field) : _db.HashSetAsync(key, field, value);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.List
         */
        public List List { get => _list.Value; }

        /*
         * 遍历列表
         */
        private Task<IEnumerable<string>> List_OnRange(string key) =>
            _db.ListRangeAsync(key).ContinueWith<IEnumerable<string>>(t => t.Result.ToStringArray());

        /*
         * 获取列表头部
         */
        private Task<string> List_OnPopLeft(string key) =>
            _db.ListLeftPopAsync(key).ContinueWith<string>(t => t.Result);

        /*
         * 获取列表尾部
         */
        private Task<string> List_OnPopRight(string key) =>
            _db.ListRightPopAsync(key).ContinueWith<string>(t => t.Result);

        /*
         * 设置列表头部
         */
        private Task List_OnPushLeft(string key, string value) =>
            value == null ? Task.CompletedTask : _db.ListLeftPushAsync(key, value);

        /*
         * 设置列表尾部
         */
        private Task List_OnPushRight(string key, string value) =>
            value == null ? Task.CompletedTask : _db.ListRightPushAsync(key, value);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.Set
         */
        public Set Set { get => _set.Value; }

        /*
         * 遍历集合
         */
        private Task<IEnumerable<string>> Set_OnRange(string key) =>
            _db.SetMembersAsync(key).ContinueWith<IEnumerable<string>>(t => t.Result.ToStringArray());

        /*
         * 添加集合项
         */
        private Task Set_OnAdd(string key, string value) =>
            value == null ? Task.CompletedTask : _db.SetAddAsync(key, value);

        /*
         * 移除集合项
         */
        private Task Set_OnRemove(string key, string value) =>
            value == null ? Task.CompletedTask : _db.SetRemoveAsync(key, value);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.Zset
         */
        public Zset Zset { get => _zset.Value; }

        /*
         * 遍历有序集合
         */
        private Task<IEnumerable<string>> Zset_OnRange(string key, bool order) =>
            _db.SortAsync(key, order: order ? Order.Ascending : Order.Descending).ContinueWith<IEnumerable<string>>(t => t.Result.ToStringArray());

        /*
         * 添加有序集合项
         */
        private Task Zset_OnAdd(string key, string value, double score) =>
            value == null ? Task.CompletedTask : _db.SortedSetAddAsync(key, value, score);

        /*
         * 移除有序集合项
         */
        private Task Zset_OnRemove(string key, string value) =>
            value == null ? Task.CompletedTask : _db.SortedSetRemoveAsync(key, value);

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.Batch(Func<IBatch, Task> task)
         */
        Task IContext.Batch(Func<IBatch, Task> task) =>
            _db switch
            {
                IDatabase db => task(new StackExchangeContext(db.CreateBatch())),
                _ => Task.FromException(new InvalidOperationException())
            };

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IContext.Batch<T>(Func<IBatch, Task<IEnumerable<T>>> task)
         */
        Task<T[]> IContext.Batch<T>(Func<IBatch, Task<T[]>> task) =>
            _db switch
            {
                IDatabase db => task(new StackExchangeContext(db.CreateBatch())),
                _ => Task.FromException<T[]>(new InvalidOperationException())
            };

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IBatch.Execute(IEnumerable<Task> tasks)
         */
        Task IBatch.Execute(params Task[] tasks)
        {
            if (_db is StackExchange.Redis.IBatch batch)
            {
                batch.Execute();
                return Task.WhenAll(tasks);
            }
            else
            {
                return Task.FromException(new InvalidOperationException());
            }
        }

        /*
         * 实现接口 Kean.Infrastructure.NoSql.Redis.IBatch.Execute<T>(IEnumerable<Task<T>> tasks)
         */
        Task<T[]> IBatch.Execute<T>(params Task<T>[] tasks)
        {
            if (_db is StackExchange.Redis.IBatch batch)
            {
                batch.Execute();
                return Task.WhenAll(tasks);
            }
            else
            {
                return Task.FromException<T[]>(new InvalidOperationException());
            }
        }

        /*
         * 实现接口 System.IDisposable.Dispose()
         */
        void IDisposable.Dispose()
        {
            if (_connection)
            {
                _db?.Multiplexer.Dispose();
            }
        }
    }
}