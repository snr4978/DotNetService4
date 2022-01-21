using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace Kean.Infrastructure.SignalR
{
    /// <summary>
    /// 抽象 Hub 映射器
    /// </summary>
    public abstract class HubMapper
    {
        /// <summary>
        /// 映射 Hub 路径
        /// </summary>
        /// <param name="endpointBuilder">路由建造者</param>
        internal abstract void Map(IEndpointRouteBuilder endpointBuilder);
    }

    /// <summary>
    /// Hub 映射器
    /// </summary>
    /// <typeparam name="T">Hub 类型</typeparam>
    public sealed class HubMapper<T> : HubMapper where T : Hub
    {
        /*
         * 实现 Kean.Infrastructure.Signalr.HubMapper.Map 方法
         */
        internal override void Map(IEndpointRouteBuilder endpointBuilder)
        {
            var pattern = typeof(T).GetCustomAttribute<RouteAttribute>()?.Pattern;
            if (string.IsNullOrEmpty(pattern))
            {
                pattern = $"/signalr/{(typeof(T).Name.EndsWith("Hub") ? typeof(T).Name[..^3] : typeof(T).Name)}";
            }
            endpointBuilder.MapHub<T>(pattern);
        }
    }
}
