using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Infrastructure.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 身份验证失败时，清理缓存
    /// </summary>
    public sealed class AuthenticateFailEventHandler_Finalize : EventHandler<AuthenticateFailEvent>
    {
        private readonly ISessionRepository _sessionRepository; // 会话仓库
        private readonly ISecurityRepository _securityRepository; // 安全仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AuthenticateFailEventHandler_Finalize(
            ISessionRepository sessionRepository,
            ISecurityRepository securityRepository)
        {
            _sessionRepository = sessionRepository;
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(AuthenticateFailEvent @event, CancellationToken cancellationToken)
        {
            if ((await _sessionRepository.GetIdentity(@event.Session)).HasValue)
            {
                await _sessionRepository.RemoveSession(@event.Session);
                await _securityRepository.WriteLog(nameof(SecurityLog.Offline), JsonHelper.Serialize(new
                {
                    @event.Session,
                    Reason = nameof(OfflineReason.Timeout)
                }));
            }
        }
    }
}
