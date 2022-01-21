using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Kean.Infrastructure.SignalR
{
    /// <summary>
    /// Hub 集合
    /// </summary>
    public sealed class HubCollection
    {
        private readonly IServiceCollection _services;
        private readonly IList<HubMapper> _list = new List<HubMapper>();

        /// <summary>
        /// 初始化 Kean.Infrastructure.SignalR.HubCollection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        internal HubCollection(IServiceCollection services) =>
            _services = services;

        /// <summary>
        /// 添加 Hub
        /// </summary>
        /// <typeparam name="THub">Hub 类型</typeparam>
        public void Add<THub>() where THub : Hub
        {
            _services.AddScoped<THub>();
            _list.Add(new HubMapper<THub>());
        }

        /// <summary>
        /// 添加 Hub
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="THub">Hub 类型</typeparam>
        public void Add<TService, THub>()
            where TService : class
            where THub : Hub, TService
        {
            _services.AddScoped<TService, THub>();
            _list.Add(new HubMapper<THub>());
        }

        /// <summary>
        /// 返回一个循环访问 Hub 映射器集合的枚举器
        /// </summary>
        /// <returns>用于循环访问集合的枚举数</returns>
        public IEnumerator<HubMapper> GetEnumerator() =>
            _list.GetEnumerator();
    }
}
