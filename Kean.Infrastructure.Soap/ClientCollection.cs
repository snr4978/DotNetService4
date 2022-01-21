using Microsoft.Extensions.DependencyInjection;

namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// 客户端集合
    /// </summary>
    public sealed class ClientCollection
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// 初始化 Kean.Infrastructure.Soap.ClientCollection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        internal ClientCollection(IServiceCollection services) =>
            _services = services;

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        public void Add<TService>()
            where TService : class =>
            _services.AddScoped<TService>();

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImplementation">业务类型</typeparam>
        public void Add<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService =>
            _services.AddScoped<TService, TImplementation>();
    }
}
