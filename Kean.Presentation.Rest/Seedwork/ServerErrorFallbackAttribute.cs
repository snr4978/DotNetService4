using Microsoft.AspNetCore.Mvc;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指示 Action 发生服务器内部错误时，可以返回指定的友好信息
    /// </summary>
    public sealed class ServerErrorFallbackAttribute : FallbackAttribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.ServerErrorFallbackAttribute 类的新实例
        /// </summary>
        /// <param name="type">返回类型：可以是 Microsoft.AspNetCore.Mvc.IActionResult 的实现，也可以是一个对象类型（这个类型需要包含一个仅接受 System.Exception 参数的构造函数）</param>
        public ServerErrorFallbackAttribute(Type type) : base(type) { }

        /// <summary>
        /// 得到一个 Microsoft.AspNetCore.Mvc.IActionResult 的实例
        /// </summary>
        /// <param name="exception">异常信息</param>
        public IActionResult Result(Exception exception) => base.Result(exception);
    }
}
