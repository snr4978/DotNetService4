using Microsoft.AspNetCore.Builder;
using OrleansDashboard;

namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// ApplicationBuilder 扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 注册 Orleans 中间件
        /// </summary>
        /// <param name="app">应用程序管道</param>
        /// <returns>应用程序管道</returns>
        public static IApplicationBuilder UseOrleans(this IApplicationBuilder app) =>
            app.Map("/orleans", a =>
            {
                a.UseMiddleware<AuthorizationMiddleware>();
                a.UseMiddleware<DashboardMiddleware>();
            });
    }
}
