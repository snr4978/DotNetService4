using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 后台服务
    /// </summary>
    public sealed class HostedService : IHostedService
    {
        private readonly RecurringJobCollection _jobs;

        /// <summary>
        /// 初始化 Kean.Infrastructure.Hangfire.HostedService 类的新实例
        /// </summary>
        /// <param name="jobs">定时作业建造者</param>
        internal HostedService(RecurringJobCollection jobs) =>
            _jobs = jobs;

        /*
         * 实现 Microsoft.Extensions.Hosting.IHostedService.StartAsync(CancellationToken cancellationToken) 方法
         */
        public Task StartAsync(CancellationToken cancellationToken) =>
            Task.Run(() =>
            {
                foreach (var item in _jobs)
                {
                    item.Build();
                }
            }, cancellationToken);

        /*
         * 实现 Microsoft.Extensions.Hosting.IHostedService.StopAsync(CancellationToken cancellationToken) 方法
         */
        public Task StopAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
