using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Models;

namespace Kean.Domain.Admin
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Domain.Admin.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<CreateRoleCommand, Role>();
            CreateMap<ModifyRoleCommand, Role>();
            CreateMap<CreateRoleCommand, CreateRoleSuccessEvent>();
            CreateMap<ModifyRoleCommand, ModifyRoleSuccessEvent>();
            CreateMap<DeleteRoleCommand, DeleteRoleSuccessEvent>();
            CreateMap<SetMenuPermissionCommand, SetMenuPermissionSuccessEvent>();

            CreateMap<CreateUserCommand, User>();
            CreateMap<ModifyUserCommand, User>();
            CreateMap<CreateUserCommand, CreateUserSuccessEvent>();
            CreateMap<ModifyUserCommand, ModifyUserSuccessEvent>();
            CreateMap<DeleteUserCommand, DeleteUserSuccessEvent>();
            CreateMap<ResetPasswordCommand, ResetPasswordSuccessEvent>();
        }
    }
}
