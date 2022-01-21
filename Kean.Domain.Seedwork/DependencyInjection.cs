using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kean.Domain.Seedwork
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Domain.Seedwork.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly()); // 注册 MediatR
            services.AddScoped<ICommandBus, CommandBus>(); // 命令总线注入
            services.AddScoped<IDomain, Domain>(); // 共享服务注入
            services.AddScoped<INotification, Notification>(); // 通知注入
        }
    }
}
