using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// ApplicationBuilder 扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 注册 Hangfire 中间件
        /// </summary>
        /// <param name="app">应用程序管道</param>
        /// <returns>应用程序管道</returns>
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app) =>
            app.UseHangfireDashboard(options: new()
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new AuthorizationFilter()
                }
            });
    }
}
