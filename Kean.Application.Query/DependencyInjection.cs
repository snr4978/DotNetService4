using Kean.Application.Query.Implements;
using Kean.Application.Query.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Application.Query
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Application.Query.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(AutoMapper));

            // Services
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
