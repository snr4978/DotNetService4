using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 后台任务基类
    /// </summary>
    /// <typeparam name="T">后台任务类型</typeparam>
    public sealed class BackgroundService<T> : BackgroundService where T : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundService<T>> _logger;
        private IServiceScope _serviceScope;
        private T _backgroundService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public BackgroundService(
            IServiceProvider serviceProvider,
            ILogger<BackgroundService<T>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        private T Service => _backgroundService ??= (_serviceScope = _serviceProvider.CreateScope()).ServiceProvider.GetRequiredService<T>();

        /*
         * 实现 System.IDisposable.Dispose() 方法
         */
        public override void Dispose()
        {
            _backgroundService.Dispose();
            _serviceScope.Dispose();
            base.Dispose();
        }

        /*
         * 实现 Microsoft.Extensions.Hosting.BackgroundService.StartAsync(CancellationToken cancellationToken) 方法
         */
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting BackgroundService {Service}", typeof(T).FullName);
            if (typeof(T).GetMethod("StartAsync").DeclaringType == typeof(T))
            {
                await Service.StartAsync(cancellationToken);
            }
            await base.StartAsync(cancellationToken);
        }

        /*
         * 实现 Microsoft.Extensions.Hosting.BackgroundService.StopAsync(CancellationToken cancellationToken) 方法
         */
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping BackgroundService {Service}", typeof(T).FullName);
            if (typeof(T).GetMethod("StopAsync").DeclaringType == typeof(T))
            {
                await Service.StopAsync(cancellationToken);
            }
            await base.StopAsync(cancellationToken);
        }

        /*
         * 实现 Microsoft.Extensions.Hosting.BackgroundService.ExecuteAsync(CancellationToken stoppingToken) 方法
         */
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var executeMethod = typeof(T).GetMethod("ExecuteAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            return executeMethod.Invoke(Service, new object[] { stoppingToken }) as Task;
        }
    }
}
