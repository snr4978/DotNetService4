using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 全局操作过滤器
    /// </summary>
    public sealed class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 在调用操作方法之前发生
        /// </summary>
        /// <param name="context">操作上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 为调用方法补充参数
            foreach (var item in context.HttpContext.Items)
            {
                context.ActionArguments[item.Key.ToString()] = item.Value;
            }
        }

        /// <summary>
        /// 在调用操作方法之后发生
        /// </summary>
        /// <param name="context">操作执行的上下文</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // 调用方法发生异常时，向调用者反馈系统错误信息
            if (context.Exception != null)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.HttpContext.Response.WriteAsync(context.Exception.ToString());
            }
        }
    }
}
