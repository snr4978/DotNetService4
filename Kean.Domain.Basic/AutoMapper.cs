using AutoMapper;
using Kean.Domain.Basic.Commands;
using Kean.Domain.Basic.Events;
using Kean.Domain.Basic.Models;

namespace Kean.Domain.Basic
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Domain.Basic.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<CreateRoleCommand, Role>();
            CreateMap<ModifyRoleCommand, Role>();
            CreateMap<DeleteRoleCommand, DeleteRoleSuccessEvent>();

            CreateMap<CreateUserCommand, User>();
            CreateMap<ModifyUserCommand, User>();
            CreateMap<DeleteUserCommand, DeleteUserSuccessEvent>();
        }
    }
}
