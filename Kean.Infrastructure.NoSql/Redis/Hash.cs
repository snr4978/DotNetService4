using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 哈希类型
    /// </summary>
    public sealed class Hash : IValue<Hash.Value>
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Redis.Hash 类的新实例
        /// </summary>
        internal Hash() { }

        /// <summary>
        /// 遍历处理
        /// </summary>
        internal event Func<string, Task<IDictionary<string, string>>> OnRange;

        /// <summary>
        /// 读值处理
        /// </summary>
        internal event Func<string, string, Task<string>> OnGet;

        /// <summary>
        /// 写值处理
        /// </summary>
        internal event Func<string, string, string, Task> OnSet;

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>哈希值</returns>
        public Value this[string key]
        {
            get
            {
                var value = new Value(key);
                value.OnRange += OnRange;
                value.OnGet += OnGet;
                value.OnSet += OnSet;
                return value;
            }
        }

        /// <summary>
        /// 哈希值
        /// </summary>
        public sealed class Value
        {
            private readonly string _key;

            /// <summary>
            /// 初始化 Kean.Infrastructure.NoSql.Redis.Hash.Value 类的新实例
            /// </summary>
            /// <param name="key"></param>
            internal Value(string key) => _key = key;

            /// <summary>
            /// 遍历处理
            /// </summary>
            internal event Func<string, Task<IDictionary<string, string>>> OnRange;

            /// <summary>
            /// 读值处理
            /// </summary>
            internal event Func<string, string, Task<string>> OnGet;

            /// <summary>
            /// 写值处理
            /// </summary>
            internal event Func<string, string, string, Task> OnSet;

            /// <summary>
            /// 遍历
            /// </summary>
            /// <returns>值</returns>
            public Task<IDictionary<string, string>> Range() => OnRange(_key);

            /// <summary>
            /// 读取字符串
            /// </summary>
            /// <param name="key">键</param>
            /// <returns>值</returns>
            public Task<string> Get(string key) => OnGet(_key, key);

            /// <summary>
            /// 写入字符串
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="value">值</param>
            public Task Set(string key, string value) => OnSet(_key, key, value);
        }
    }
}
