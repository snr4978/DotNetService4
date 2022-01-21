using Microsoft.Extensions.Hosting;
using Orleans.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// 后台服务
    /// </summary>
    public sealed class HostedService : IHostedService
    {
        private readonly ISiloHost _siloHost;

        /// <summary>
        /// 初始化 Kean.Infrastructure.Orleans.HostedService 类的新实例
        /// </summary>
        /// <param name="siloHost">筒仓服务</param>
        internal HostedService(ISiloHost siloHost) =>
            _siloHost = siloHost;

        /*
         * 实现 Microsoft.Extensions.Hosting.IHostedService.StartAsync(CancellationToken cancellationToken) 方法
         */
        public Task StartAsync(CancellationToken cancellationToken) =>
            _siloHost.StartAsync(cancellationToken);

        /*
         * 实现 Microsoft.Extensions.Hosting.IHostedService.StopAsync(CancellationToken cancellationToken) 方法
         */
        public Task StopAsync(CancellationToken cancellationToken) =>
            _siloHost.StopAsync(cancellationToken);
    }
}
