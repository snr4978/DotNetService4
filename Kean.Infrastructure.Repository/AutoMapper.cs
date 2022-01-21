using AutoMapper;
using Kean.Infrastructure.Database.Repository.Default.Entities;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Application.Repository.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<Domain.Basic.Models.Role, T_SYS_ROLE>()
                .ForMember(entity => entity.ROLE_ID, model => model.MapFrom(role => role.Id))
                .ForMember(entity => entity.ROLE_NAME, model => model.MapFrom(role => role.Name))
                .ForMember(entity => entity.ROLE_REMARK, model => model.MapFrom(role => role.Remark));

            CreateMap<Domain.Basic.Models.User, T_SYS_USER>()
                .ForMember(entity => entity.USER_ID, model => model.MapFrom(user => user.Id))
                .ForMember(entity => entity.USER_NAME, model => model.MapFrom(user => user.Name))
                .ForMember(entity => entity.USER_ACCOUNT, model => model.MapFrom(user => user.Account));
        }
    }
}
