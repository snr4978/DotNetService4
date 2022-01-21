using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SoapCore;
using System.Reflection;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// 抽象 Soap 映射器
    /// </summary>
    public abstract class SoapMapper
    {
        /// <summary>
        /// 映射 Soap 路径
        /// </summary>
        /// <param name="endpointBuilder">路由建造者</param>
        internal abstract void Map(IEndpointRouteBuilder endpointBuilder);
    }

    /// <summary>
    /// Soap 映射器
    /// </summary>
    /// <typeparam name="T">服务类型</typeparam>
    public sealed class SoapMapper<T> : SoapMapper
    {
        /*
         * 实现 Kean.Infrastructure.Soap.SoapMapper.Map 方法
         */
        internal override void Map(IEndpointRouteBuilder endpointBuilder)
        {
            var path = typeof(T).GetCustomAttribute<RouteAttribute>()?.Path;
            if (string.IsNullOrEmpty(path))
            {
                path = $"/soap/{(typeof(T).Name.EndsWith("Service") ? typeof(T).Name[..^7] : typeof(T).Name).ToLower()}";
            }
            endpointBuilder.UseSoapEndpoint<T>(options => options.Path = path.StartsWith('/') ? path : $"/{path}")
                .WithMetadata(new SoapMetadata());
        }
    }
}
