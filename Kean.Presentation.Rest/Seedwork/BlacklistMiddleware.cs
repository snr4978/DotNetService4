using Kean.Application.Query.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 黑名单中间件
    /// </summary>
    public sealed class BlacklistMiddleware
    {
        private readonly RequestDelegate _next; // 管道

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.BlacklistMiddleware 类的新实例
        /// </summary>
        public BlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        public async Task InvokeAsync(HttpContext context, IAppService service)
        {
            var ip = context.Request.Headers["X-Real-IP"].FirstOrDefault() 
                ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (await service.GetBlacklist(ip) == null)
            {
                context.Items["ip"] = ip;
                context.Items["ua"] = context.Request.Headers["User-Agent"].ToString();
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 403;
            }
        }
    }
}
