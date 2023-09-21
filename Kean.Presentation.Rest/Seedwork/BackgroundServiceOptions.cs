using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 后台任务选项
    /// </summary>
    public sealed class BackgroundServiceOptions
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.BackgroundServiceOptions 的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public BackgroundServiceOptions(IServiceCollection services) => _services = services;

        /// <summary>
        /// 寄宿后台任务
        /// </summary>
        /// <typeparam name="T">后台任务类型</typeparam>
        public void Host<T>() where T : BackgroundService
        {
            _services.AddScoped<T>().AddHostedService<BackgroundService<T>>();
        }

        /// <summary>
        /// 寄宿后台任务
        /// </summary>
        /// <typeparam name="T">后台任务类型</typeparam>
        /// <param name="accessor">访问器选项</param>
        public void Host<T>(Action<BackgroundServiceAccessorOptions<T>> accessor) where T : BackgroundService
        {
            Host<T>();
            accessor.Invoke(new BackgroundServiceAccessorOptions<T>(_services));
        }

        /// <summary>
        /// 后台任务访问器选项
        /// </summary>
        /// <typeparam name="TService">后台任务类型</typeparam>
        public sealed class BackgroundServiceAccessorOptions<TService> where TService : BackgroundService
        {
            private readonly IServiceCollection _services;

            /// <summary>
            /// 初始化 Kean.Presentation.Rest.BackgroundServiceOptions.BackgroundServiceAccessorOptions 的新实例
            /// </summary>
            /// <param name="services">服务描述符</param>
            internal BackgroundServiceAccessorOptions(IServiceCollection services) => _services = services;

            /// <summary>
            /// 附加访问器类型
            /// </summary>
            /// <typeparam name="TAccessor">访问器类型</typeparam>
            public BackgroundServiceAccessorOptions<TService> With<TAccessor>() where TAccessor : class
            {
                _services.AddScoped(serviceProvider => (serviceProvider.GetRequiredService<IEnumerable<IHostedService>>()
                    .First(s => s is BackgroundService<TService>) as BackgroundService<TService>).Service as TAccessor);
                return this;
            }
        }
    }
}
