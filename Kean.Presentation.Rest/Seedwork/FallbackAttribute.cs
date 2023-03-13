using Microsoft.AspNetCore.Mvc;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指示 Action 上下文异常时，可以返回指定的友好信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class FallbackAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.FallbackAttribute 类的新实例
        /// </summary>
        /// <param name="type">返回类型：可以是 Microsoft.AspNetCore.Mvc.IActionResult 的实现，也可以是一个对象类型</param>
        protected FallbackAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// 返回类型：可以是 Microsoft.AspNetCore.Mvc.IActionResult 的实现，也可以是一个对象类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 得到一个 Microsoft.AspNetCore.Mvc.IActionResult 的实例
        /// </summary>
        /// <param name="args">构造参数</param>
        /// <returns>结果实例</returns>
        protected IActionResult Result(params object[] args)
        {
            var instance = Activator.CreateInstance(Type, args);
            return instance as IActionResult ?? new OkObjectResult(instance);
        }
    }
}
