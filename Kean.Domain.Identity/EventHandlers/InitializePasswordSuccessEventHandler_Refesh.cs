using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 初始化密码成功时，刷新缓存密码策略信息
    /// </summary>
    public sealed class InitializePasswordSuccessEventHandler_Refesh : EventHandler<InitializePasswordSuccessEvent>
    {
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public InitializePasswordSuccessEventHandler_Refesh(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(InitializePasswordSuccessEvent @event, CancellationToken cancellationToken)
        {
            await _userRepository.PasswordInitial(@event.Id);
            await _userRepository.PasswordExpired(@event.Id);
        }
    }
}
