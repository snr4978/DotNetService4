using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Infrastructure.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 注销时，处理安全策略
    /// </summary>
    public sealed class LogoutSuccessEventHandler_Security : EventHandler<LogoutSuccessEvent>
    {
        private readonly ISecurityRepository _securityRepository; // 安全仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LogoutSuccessEventHandler_Security(
            ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LogoutSuccessEvent @event, CancellationToken cancellationToken)
        {
            await _securityRepository.WriteLog(nameof(SecurityLog.Offline), JsonHelper.Serialize(new
            {
                @event.Session,
                @event.Reason
            }));
        }
    }
}
