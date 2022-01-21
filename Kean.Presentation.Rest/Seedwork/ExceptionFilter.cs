using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger; // 日志

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.ExceptionFilter 类的新实例
        /// </summary>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 出现异常时发生
        /// </summary>
        /// <param name="context">发生异常的上下文</param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "{Method} '{Path}' Exception:", context.HttpContext.Request.Method, context.HttpContext.Request.Path);
            context.ExceptionHandled = true;
        }
    }
}
