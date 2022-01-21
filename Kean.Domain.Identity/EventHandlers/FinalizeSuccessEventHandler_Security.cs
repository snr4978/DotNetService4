using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Infrastructure.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 终结成功时，处理安全策略
    /// </summary>
    public sealed class FinalizeSuccessEventHandler_Security : EventHandler<FinalizeSuccessEvent>
    {
        private readonly ISecurityRepository _securityRepository; // 安全仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public FinalizeSuccessEventHandler_Security(
            ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(FinalizeSuccessEvent @event, CancellationToken cancellationToken)
        {
            foreach (var item in @event.Session)
            {
                await _securityRepository.WriteLog(nameof(SecurityLog.Offline), JsonHelper.Serialize(new
                {
                    Session = item,
                    Reason = nameof(OfflineReason.Restart)
                }));
            }
        }
    }
}
