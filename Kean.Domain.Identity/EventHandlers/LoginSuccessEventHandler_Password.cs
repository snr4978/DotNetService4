using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 登录成功时，缓存密码策略信息
    /// </summary>
    public sealed class LoginSuccessEventHandler_Password : EventHandler<LoginSuccessEvent>
    {
        private readonly IUserRepository _userRepository; // 用户仓库
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LoginSuccessEventHandler_Password(
            IUserRepository userRepository,
            ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoginSuccessEvent @event, CancellationToken cancellationToken)
        {
            var identity = await _sessionRepository.GetIdentity(@event.Session);
            if (identity.HasValue)
            {
                await _userRepository.PasswordInitial(identity.Value);
                await _userRepository.PasswordExpired(identity.Value);
            }
        }
    }
}
