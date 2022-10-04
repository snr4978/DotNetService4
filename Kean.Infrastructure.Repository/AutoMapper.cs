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
            CreateMap<Domain.Admin.Models.Role, T_SYS_ROLE>()
                .ForMember(entity => entity.ROLE_ID, options => options.MapFrom(model => model.Id))
                .ForMember(entity => entity.ROLE_NAME, options => options.MapFrom(model => model.Name))
                .ForMember(entity => entity.ROLE_REMARK, options => options.MapFrom(model => model.Remark));

            CreateMap<Domain.Admin.Models.User, T_SYS_USER>()
                .ForMember(entity => entity.USER_ID, options => options.MapFrom(model => model.Id))
                .ForMember(entity => entity.USER_NAME, options => options.MapFrom(model => model.Name))
                .ForMember(entity => entity.USER_ACCOUNT, options => options.MapFrom(model => model.Account));

        }
    }
}
