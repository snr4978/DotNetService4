using AutoMapper;
using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Events;

namespace Kean.Domain.Identity
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Domain.Identity.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<LoginCommand, LoginSuccessEvent>();
            CreateMap<LoginCommand, LoginFailEvent>();
            CreateMap<LogoutCommand, LogoutSuccessEvent>();
            CreateMap<InitializePasswordCommand, InitializePasswordSuccessEvent>();
            CreateMap<ModifyPasswordCommand, ModifyPasswordSuccessEvent>();
        }
    }
}
