using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
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
        /// 向服务描述中追加启动过滤
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection Startup(this IServiceCollection services)
        {
            return services.AddTransient<IStartupFilter, StartupFilter>();
        }
    }
}
