using AutoMapper;
using Kean.Application.Command.ViewModels;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Identity.Commands;
using Kean.Domain.Message.Commands;

namespace Kean.Application.Command
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Application.Command.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<User, LoginCommand>();
            CreateMap<User, ModifyAvatarCommand>();
            CreateMap<Password, InitializePasswordCommand>();
            CreateMap<Password, ModifyPasswordCommand>();
            CreateMap<Message, SendMessageCommand>();
            CreateMap<Role, CreateRoleCommand>();
            CreateMap<Role, ModifyRoleCommand>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<User, ModifyUserCommand>();
        }
    }
}
