using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Infrastructure.Orleans
{
    /// <summary>
    /// Orleans 配置项
    /// </summary>
    public sealed class OrleansOptions
    {
        /// <summary>
        /// 筒仓端口（silo-to-silo）
        /// </summary>
        public int SiloPort { get; set; } = 11111;

        /// <summary>
        /// 网关端口（silo-to-client）
        /// </summary>
        public int GatewayPort { get; set; } = 30000;

        /// <summary>
        /// 集群 ID
        /// </summary>
        public string ClusterId { get; set; } = "orleans-cluster-kean";

        /// <summary>
        /// 服务 ID
        /// </summary>
        public string ServiceId { get; set; } = "orleans-service-kean";

        /// <summary>
        /// Redis
        /// </summary>
        public RedisClustering RedisClustering { get; set; } = new();

        /// <summary>
        /// 配置委托
        /// </summary>
        internal Action<IServiceCollection> ConfigureDelegate { get; private set; }

        /// <summary>
        /// 配置服务描述
        /// </summary>
        /// <param name="configureDelegate">配置委托</param>
        public void ConfigureServices(Action<IServiceCollection> configureDelegate) =>
            ConfigureDelegate = configureDelegate;
    }
}
