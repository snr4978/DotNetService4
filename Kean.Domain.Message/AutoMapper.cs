using AutoMapper;
using Kean.Domain.Message.Commands;
using Kean.Domain.Message.Events;

namespace Kean.Domain.Message
{
    /// <summary>
    /// 模型映射配置
    /// </summary>
    public class AutoMapper : Profile
    {
        /// <summary>
        /// 初始化 Kean.Domain.Message.AutoMapper 类的新实例
        /// </summary>
        public AutoMapper()
        {
            CreateMap<SendMessageCommand, SendMessageSuccessEvent>();
        }
    }
}
