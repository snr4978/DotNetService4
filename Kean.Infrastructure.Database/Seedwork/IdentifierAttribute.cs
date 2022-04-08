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
        public IdentifierAttribute() => Increment = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="increment">是否自增</param>
        public IdentifierAttribute(bool increment) => Increment = increment;

        /// <summary>
        /// 获取主键是否是自增主键
        /// </summary>
        public bool Increment { get; }
    }
}
