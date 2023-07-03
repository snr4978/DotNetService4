using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务描述中追加启动过滤
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection Startup(this IServiceCollection services)
        {
            return services.AddTransient<IStartupFilter, StartupFilter>();
        }

        /// <summary>
        /// 向服务描述中追加配置类型
        /// </summary>
        /// <typeparam name="T">包含配置操作的类型</typeparam>
        /// <param name="services">服务描述符</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection Add<T>(this IServiceCollection services)
        {
            Activator.CreateInstance(typeof(T), services);
            return services;
        }

        /// <summary>
        /// 对 Microsoft.Extensions.DependencyInjection.MvcServiceCollectionExtensions.AddControllers 的扩展，固化一些 IMvcBuilder 的后续操作
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="configure">配置选项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddControllers(this IServiceCollection services, Action<MvcOptions> configure)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return MvcServiceCollectionExtensions.AddControllers(services, configure)
                .AddXmlSerializerFormatters()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                        (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(a => a is BadRequestFallbackAttribute) as BadRequestFallbackAttribute)?.Result() ?? new BadRequestResult();
                })
                .Services;
        }

        /// <summary>
        /// 向服务描述中追加后台任务配置
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="configure">配置选项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddBackground(this IServiceCollection services, Action<BackgroundServiceOptions> configure)
        {
            var options = new BackgroundServiceOptions(services);
            configure.Invoke(options);
            return services;
        }
    }
}
