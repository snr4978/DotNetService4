using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// 服务端集合
    /// </summary>
    public sealed class ServerCollection
    {
        private readonly IServiceCollection _services;
        private readonly IList<SoapMapper> _list = new List<SoapMapper>();

        /// <summary>
        /// 初始化 Kean.Infrastructure.Soap.ServerCollection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        internal ServerCollection(IServiceCollection services) =>
            _services = services;

        /// <summary>
        /// 添加服务端
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        public void Add<TService>() 
            where TService : class
        {
            _services.AddScoped<TService>();
            _list.Add(new SoapMapper<TService>());
        }

        /// <summary>
        /// 添加服务端
        /// </summary>
        /// <typeparam name="TContract">契约类型</typeparam>
        /// <typeparam name="TImplementation">业务类型</typeparam>
        public void Add<TContract, TImplementation>()
            where TContract : class
            where TImplementation : class, TContract
        {
            _services.AddScoped<TContract, TImplementation>();
            _list.Add(new SoapMapper<TImplementation>());
        }

        /// <summary>
        /// 返回一个循环访问 Soap 映射器集合的枚举器
        /// </summary>
        /// <returns>用于循环访问集合的枚举数</returns>
        public IEnumerator<SoapMapper> GetEnumerator() =>
            _list.GetEnumerator();
    }
}
