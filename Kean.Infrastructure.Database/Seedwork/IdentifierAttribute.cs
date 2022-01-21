using System;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表示对象的唯一标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IdentifierAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IdentifierAttribute() => Identity = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="identity">是否自增</param>
        public IdentifierAttribute(bool identity) => Identity = identity;

        /// <summary>
        /// 获取主键是否是自增主键
        /// </summary>
        public bool Identity { get; }
    }
}
