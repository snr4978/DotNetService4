using System;

namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 表示对象是由计算得出的，不支持写操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ComputedAttribute : Attribute
    {

    }
}
