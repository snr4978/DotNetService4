using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// EndpointRouteBuilder 扩展方法
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// 映射 Soap 路径
        /// </summary>
        /// <param name="endpoints">终节点路由</param>
        /// <returns>终节点路由</returns>
        public static IEndpointRouteBuilder MapSoaps(this IEndpointRouteBuilder endpoints)
        {
            foreach (var item in endpoints.ServiceProvider.GetService<ServiceCollection>())
            {
                item.Map(endpoints);
            }
            return endpoints;
        }
    }
}
