using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 集合类型
    /// </summary>
    public sealed class Set : IValue<Set.Value>
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Redis.Set 类的新实例
        /// </summary>
        internal Set() { }

        /// <summary>
        /// 遍历处理
        /// </summary>
        internal event Func<string, Task<IEnumerable<string>>> OnRange;

        /// <summary>
        /// 添加处理
        /// </summary>
        internal event Func<string, string, Task> OnAdd;

        /// <summary>
        /// 移除处理
        /// </summary>
        internal event Func<string, string, Task> OnRemove;

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>集合</returns>
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
        /// 集合值
        /// </summary>
        public sealed class Value
        {
            private readonly string _key;

            /// <summary>
            /// 初始化 Kean.Infrastructure.NoSql.Redis.Set.Value 类的新实例
            /// </summary>
            /// <param name="key"></param>
            internal Value(string key) => _key = key;

            /// <summary>
            /// 遍历处理
            /// </summary>
            internal event Func<string, Task<IEnumerable<string>>> OnRange;

            /// <summary>
            /// 添加处理
            /// </summary>
            internal event Func<string, string, Task> OnAdd;

            /// <summary>
            /// 移除处理
            /// </summary>
            internal event Func<string, string, Task> OnRemove;

            /// <summary>
            /// 遍历
            /// </summary>
            /// <returns>值</returns>
            public Task<IEnumerable<string>> Range() => OnRange(_key);

            /// <summary>
            /// 添加项
            /// </summary>
            /// <param name="value">值</param>
            public Task Add(string value) => OnAdd(_key, value);

            /// <summary>
            /// 移除项
            /// </summary>
            /// <param name="value">值</param>
            public Task Remove(string value) => OnRemove(_key, value);
        }
    }
}
