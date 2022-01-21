using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指示 Action 允许匿名访问，不进行身份验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AnonymousAttribute : Attribute
    {
    }
}
