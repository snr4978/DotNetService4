using Microsoft.AspNetCore.Mvc;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指示 Action 请求内容出错时，可以返回指定的友好信息
    /// </summary>
    public sealed class BadRequestFallbackAttribute : FallbackAttribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.BadRequestFallbackAttribute 类的新实例
        /// </summary>
        /// <param name="type">返回类型：可以是 Microsoft.AspNetCore.Mvc.IActionResult 的实现，也可以是一个对象类型</param>
        public BadRequestFallbackAttribute(Type type) : base(type) { }

        /// <summary>
        /// 得到一个 Microsoft.AspNetCore.Mvc.IActionResult 的实例
        /// </summary>
        public IActionResult Result() => base.Result();
    }
}
