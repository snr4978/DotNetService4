using Kean.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public sealed class DependencyInjection
    {
        /// <summary>
        /// 初始化 Kean.Infrastructure.Repository.DependencyInjection 类的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public DependencyInjection(IServiceCollection services)
        {
            // 映射
            services.AddAutoMapper(typeof(AutoMapper));

            // 工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 仓库
            services.AddScoped<Domain.App.Repositories.IParamRepository, ParamRepository>();
            services.AddScoped<Domain.App.Repositories.ISecurityRepository, SecurityRepository>();
            services.AddScoped<Domain.Identity.Repositories.ISecurityRepository, SecurityRepository>();
            services.AddScoped<Domain.Identity.Repositories.ISessionRepository, SessionRepository>();
            services.AddScoped<Domain.Identity.Repositories.IUserRepository, UserRepository>();
            services.AddScoped<Domain.Message.Repositories.IMessageRepository, MessageRepository>();
            services.AddScoped<Domain.Basic.Repositories.IRoleRepository, RoleRepository>();
            services.AddScoped<Domain.Basic.Repositories.IUserRepository, UserRepository>();
        }
    }
}
