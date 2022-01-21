using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 授权过滤
    /// </summary>
    internal sealed class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        private const string USERNAME = "hangfire";
        private const string PASSWORD = "hangfire";

        /*
         * 实现 Hangfire.Dashboard.IDashboardAuthorizationFilter.Authorize([NotNull] DashboardContext context) 方法
         */
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
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
            return true;
        }

        /*
         * 授权失败
         */
        private bool Challenge(HttpContext httpContext)
        {
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            return false;
        }
    }
}
