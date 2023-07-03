using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kean.Domain
{
    /// <summary>
    /// 中介者注册扩展
    /// 在使用 MediatR 注册后，按照 Kean.Domain.EventHandlerIndexAttribute 修改注入顺序
    /// </summary>
    public static class Register
    {
        /// <summary>
        /// 注册当前 Domain
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // MediatR 注册当前程序集
            var assembly = Assembly.GetCallingAssembly();
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            // 提取事件处理程序
            var serviceType = typeof(INotificationHandler<MediatR.INotification>);
            var notificationHandler = new Stack<ServiceDescriptor>();
            while (true)
            {
                var serviceDescriptor = services.LastOrDefault();
                if (serviceDescriptor == null)
                {
                    break;
                }
                else
                {
                    if (serviceDescriptor.ServiceType.IsAssignableFrom(serviceType))
                    {
                        services.Remove(serviceDescriptor);
                        notificationHandler.Push(serviceDescriptor);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // 对事件处理程序排序并重新注入
            foreach (var item in notificationHandler.OrderBy(n => n.ImplementationType.GetCustomAttribute<EventHandlerIndexAttribute>()?.Index ?? uint.MaxValue))
            {
                services.AddTransient(item.ServiceType, item.ImplementationType);
            }
            notificationHandler.Clear();
            return services;
        }
    }
}
