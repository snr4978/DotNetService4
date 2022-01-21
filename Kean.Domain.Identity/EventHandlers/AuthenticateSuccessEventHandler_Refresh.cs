using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 身份验证成功时，刷新时间戳
    /// </summary>
    public sealed class AuthenticateSuccessEventHandler_Refresh : EventHandler<AuthenticateSuccessEvent>
    {
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AuthenticateSuccessEventHandler_Refresh(
            ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(AuthenticateSuccessEvent @event, CancellationToken cancellationToken)
        {
            await _sessionRepository.UpdateSession(@event.Session);
        }
    }
}
