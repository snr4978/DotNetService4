using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// 授权中间件
    /// </summary>
    public sealed class AuthorizationMiddleware
    {
        private const string USERNAME = "orleans";
        private const string PASSWORD = "orleans";

        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化 Kean.Infrastructure.Orleans.AuthenticationMiddleware 类的新实例
        /// </summary>
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        public Task Invoke(HttpContext httpContext)
        {
            var authorization = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorization))
            {
                return Challenge(httpContext);
            }
            var values = AuthenticationHeaderValue.Parse(authorization);
            if (!"Basic".Equals(values.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                return Challenge(httpContext);
            }
            var parameters = Encoding.UTF8.GetString(Convert.FromBase64String(values.Parameter)).Split(':');
            if (parameters.Length < 2 || parameters[0] != USERNAME || parameters[1] != PASSWORD)
            {
                return Challenge(httpContext);
            }
            return _next(httpContext);
        }

        /*
         * 授权失败
         */
        private Task Challenge(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Orleans Dashboard\"");
            return Task.CompletedTask;
        }
    }
}
