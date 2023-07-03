using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// ServiceCollection 扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务描述中追加 Hangfire 配置
        /// </summary>
        /// <param name="services">服务描述符</param>
        /// <param name="setupAction">配置项</param>
        /// <returns>服务描述符</returns>
        public static IServiceCollection AddHangfire(this IServiceCollection services, Action<HangfireOptions> setupAction)
        {
            var options = new HangfireOptions();
            setupAction.Invoke(options);
            return services
                .AddHangfire(configuration =>
                {
                    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseRedisStorage(options.RedisStorage.ConnectionString, new() { Db = options.RedisStorage.Database });
                })
                .AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1))
                .AddHostedService(serviceProvider => new HostedService(options.RecurringJobs));
        }
    }
}
