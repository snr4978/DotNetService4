using System;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 字符串类型
    /// </summary>
    public sealed class String : IValue<String.Value>
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Redis.String 类的新实例
        /// </summary>
        internal String() { }

        /// <summary>
        /// 读值处理
        /// </summary>
        internal event Func<string, Task<string>> OnGet;

        /// <summary>
        /// 写值处理
        /// </summary>
        internal event Func<string, string, Task> OnSet;

        /// <summary>
        /// 获取字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字符串值</returns>
        public Value this[string key]
        {
            get
            {
                var value = new Value(key);
                value.OnGet += OnGet;
                value.OnSet += OnSet;
                return value;
            }
        }

        /// <summary>
        /// 字符串值
        /// </summary>
        public sealed class Value
        {
            private readonly string _key;

            /// <summary>
            /// 初始化 Kean.Infrastructure.NoSql.Redis.String.Value 类的新实例
            /// </summary>
            /// <param name="key"></param>
            internal Value(string key) => _key = key;

            /// <summary>
            /// 读值处理
            /// </summary>
            internal event Func<string, Task<string>> OnGet;

            /// <summary>
            /// 写值处理
            /// </summary>
            internal event Func<string, string, Task> OnSet;

            /// <summary>
            /// 读取字符串值
            /// </summary>
            /// <returns>值</returns>
            public Task<string> Get() => OnGet(_key);

            /// <summary>
            /// 写入字符串值
            /// </summary>
            /// <param name="value">值</param>
            public Task Set(string value) => OnSet(_key, value);
        }
    }
}
