using Microsoft.AspNetCore.Builder;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// ApplicationBuilder 扩展方法
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 注册 Swagger 及 UI 中间件
        /// </summary>
        /// <param name="app">应用程序管道</param>
        /// <returns>应用程序管道</returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app) => app
                .UseSwagger(options => options.RouteTemplate = "api/{documentName}/swagger.json")
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api";
                    options.HeadContent = "<style>.topbar,.url{display:none;}</style>";
                    options.DefaultModelsExpandDepth(-1);
                });

        /// <summary>
        /// 注册身份验证中间件
        /// </summary>
        /// <param name="app">应用程序管道</param>
        /// <returns>应用程序管道</returns>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app) => 
            app.UseMiddleware<AuthenticationMiddleware>();

        /// <summary>
        /// 注册黑名单中间件
        /// </summary>
        /// <param name="app">应用程序管道</param>
        /// <returns>应用程序管道</returns>
        public static IApplicationBuilder UseBlacklist(this IApplicationBuilder app) => 
            app.UseMiddleware<BlacklistMiddleware>();
    }
}
