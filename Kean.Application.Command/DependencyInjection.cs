using Kean.Application.Command.Implements;
using Kean.Application.Command.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Application.Command
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Application.Command.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapper));

            // Services
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IBasicService, BasicService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
