using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 列表类型
    /// </summary>
    public sealed class List : IValue<List.Value>
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Redis.List 类的新实例
        /// </summary>
        internal List() { }

        /// <summary>
        /// 遍历处理
        /// </summary>
        internal event Func<string, Task<IEnumerable<string>>> OnRange;

        /// <summary>
        /// 头部读值处理
        /// </summary>
        internal event Func<string, Task<string>> OnPopLeft;

        /// <summary>
        /// 尾部读值处理
        /// </summary>
        internal event Func<string, Task<string>> OnPopRight;

        /// <summary>
        /// 头部写值处理
        /// </summary>
        internal event Func<string, string, Task> OnPushLeft;

        /// <summary>
        /// 尾部写值处理
        /// </summary>
        internal event Func<string, string, Task> OnPushRight;

        /// <summary>
        /// 获取列表值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>列表值</returns>
        public Value this[string key]
        {
            get
            {
                var value = new Value(key);
                value.OnRange += OnRange;
                value.OnPopLeft += OnPopLeft;
                value.OnPopRight += OnPopRight;
                value.OnPushLeft += OnPushLeft;
                value.OnPushRight += OnPushRight;
                return value;
            }
        }

        /// <summary>
        /// 列表值
        /// </summary>
        public sealed class Value
        {
            private readonly string _key;

            /// <summary>
            /// 初始化 Kean.Infrastructure.NoSql.Redis.List.Value 类的新实例
            /// </summary>
            /// <param name="key"></param>
            internal Value(string key) => _key = key;

            /// <summary>
            /// 遍历处理
            /// </summary>
            internal event Func<string, Task<IEnumerable<string>>> OnRange;

            /// <summary>
            /// 头部读值处理
            /// </summary>
            internal event Func<string, Task<string>> OnPopLeft;

            /// <summary>
            /// 尾部读值处理
            /// </summary>
            internal event Func<string, Task<string>> OnPopRight;

            /// <summary>
            /// 头部写值处理
            /// </summary>
            internal event Func<string, string, Task> OnPushLeft;

            /// <summary>
            /// 尾部写值处理
            /// </summary>
            internal event Func<string, string, Task> OnPushRight;

            /// <summary>
            /// 遍历列表
            /// </summary>
            /// <returns>值</returns>
            public Task<IEnumerable<string>> Range() => OnRange(_key);

            /// <summary>
            /// 读取头部字符串
            /// </summary>
            /// <returns>值</returns>
            public Task<string> PopLeft() => OnPopLeft(_key);

            /// <summary>
            /// 读取尾部字符串
            /// </summary>
            /// <returns>值</returns>
            public Task<string> PopRight() => OnPopRight(_key);

            /// <summary>
            /// 写入头部字符串
            /// </summary>
            /// <param name="value">值</param>
            public Task PushLeft(string value) => OnPushLeft(_key, value);

            /// <summary>
            /// 写入尾部字符串
            /// </summary>
            /// <param name="value">值</param>
            public Task PushRight(string value) => OnPushRight(_key, value);
        }
    }
}
