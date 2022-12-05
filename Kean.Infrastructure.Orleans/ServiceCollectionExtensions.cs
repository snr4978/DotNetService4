using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Statistics;
using Serilog;
using System;

namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务描述中追加 Orleans 配置
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="setupAction">配置项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddOrleans(this IServiceCollection services, Action<OrleansOptions> setupAction)
        {
            var options = new OrleansOptions();
            setupAction(options);
            var siloHost = new SiloHostBuilder()
                .ConfigureServices(options.ConfigureDelegate)
                .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
                {
                    loggingBuilder.AddSerilog();
                    loggingBuilder.AddConfiguration(hostBuilderContext.Configuration);
                })
                .UseLocalhostClustering(
                    siloPort: options.SiloPort,
                    gatewayPort: options.GatewayPort,
                    serviceId: options.ServiceId,
                    clusterId: options.ClusterId
                )
                .UseRedisClustering(o =>
                {
                    o.ConnectionString = options.RedisClustering.ConnectionString;
                    o.Database = options.RedisClustering.Database;
                })
                .UseDashboard(options => options.HostSelf = false)
                .UsePerfCounterEnvironmentStatistics()
                .Build();
            var client = siloHost.Services.GetRequiredService<IClusterClient>();
            return services
                .AddHostedService(serviceProvider => new HostedService(siloHost))
                .AddServicesForSelfHostedDashboard()
                .AddDashboardEmbeddedFiles()
                .AddSingleton<IGrainFactory>(client)
                .AddSingleton(client);
        }
    }
}
