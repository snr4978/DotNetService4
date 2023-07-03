using Microsoft.Extensions.DependencyInjection;
using SoapCore;
using System;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务描述中追加 Soap 配置
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="setupAction">配置项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddSoap(this IServiceCollection services, Action<SoapOptions> setupAction)
        {
            services.AddSoapCore();
            var options = new SoapOptions
            {
                Services = new(services)
            };
            setupAction.Invoke(options);
            return services.AddTransient(_ => options.Services);
        }
    }
}
