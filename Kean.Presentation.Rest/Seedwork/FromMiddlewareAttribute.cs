using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指定参数仅来自中间件
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FromMiddlewareAttribute : Attribute
    {
    }
}
