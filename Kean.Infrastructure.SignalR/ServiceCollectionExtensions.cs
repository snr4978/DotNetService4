using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Infrastructure.SignalR
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务描述中追加 SignalR 配置
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="setupAction">配置项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddSignalR(this IServiceCollection services, Action<SignalROptions> setupAction)
        {
            services.AddSignalR();
            var options = new SignalROptions { Hubs = new(services) };
            setupAction.Invoke(options);
            return services.AddTransient(_ => options.Hubs);
        }
    }
}
