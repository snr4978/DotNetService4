using Kean.Infrastructure.Database.Repository.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Infrastructure.Database.Repository
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.Database.Repository.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IDatabaseCollection, DatabaseCollection>(); // 数据库集合注入

            services.AddScoped<IDefaultDb, DefaultDb>(); // 默认库
        }
    }
}
