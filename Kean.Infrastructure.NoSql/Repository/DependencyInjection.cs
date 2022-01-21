using Kean.Infrastructure.NoSql.Repository.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Infrastructure.NoSql.Repository
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.NoSql.Repository.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<IDefaultRedis, DefaultRedis>(); // 系统缓存
        }
    }
}
