using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 有序集合类型
    /// </summary>
    public sealed class Zset : IValue<Zset.Value>
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Redis.Zset 类的新实例
        /// </summary>
        internal Zset() { }

        /// <summary>
        /// 遍历处理
        /// </summary>
        internal event Func<string, bool, Task<IEnumerable<string>>> OnRange;

        /// <summary>
        /// 添加处理
        /// </summary>
        internal event Func<string, string, double, Task> OnAdd;

        /// <summary>
        /// 移除处理
        /// </summary>
        internal event Func<string, string, Task> OnRemove;

        /// <summary>
        /// 获取有序集合
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>有序集合</returns>
        public Value this[string key]
        {
            get
            {
                var value = new Value(key);
                value.OnRange += OnRange;
                value.OnAdd += OnAdd;
                value.OnRemove += OnRemove;
                return value;
            }
        }

        /// <summary>
        /// 有序集合值
        /// </summary>
        public sealed class Value
        {
            private readonly string _key;

            /// <summary>
            /// 初始化 Kean.Infrastructure.NoSql.Redis.Zset.Value 类的新实例
            /// </summary>
            /// <param name="key"></param>
            internal Value(string key) => _key = key;

            /// <summary>
            /// 遍历处理
            /// </summary>
            internal event Func<string, bool, Task<IEnumerable<string>>> OnRange;

            /// <summary>
            /// 添加处理
            /// </summary>
            internal event Func<string, string, double, Task> OnAdd;

            /// <summary>
            /// 移除处理
            /// </summary>
            internal event Func<string, string, Task> OnRemove;

            /// <summary>
            /// 遍历
            /// </summary>
            /// <param name="order">排序。如果为 True，表示升序；否则表示降序</param>
            /// <returns>值</returns>
            public Task<IEnumerable<string>> Range(bool order = true) => OnRange(_key, order);

            /// <summary>
            /// 添加项
            /// </summary>
            /// <param name="value">值</param>
            /// <param name="score">权重</param>
            public Task Add(string value, double score = 0) => OnAdd(_key, value, score);

            /// <summary>
            /// 移除项
            /// </summary>
            /// <param name="value">值</param>
            public Task Remove(string value) => OnRemove(_key, value);
        }
    }
}
