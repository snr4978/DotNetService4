using Microsoft.Extensions.DependencyInjection;

namespace Kean.Domain.App
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Domain.App.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            services.AddDomain();

            services.AddScoped<Shared.IAppService, SharedService>();
        }
    }
}
