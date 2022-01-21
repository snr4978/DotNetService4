using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Infrastructure.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 登录成功时，处理安全策略
    /// </summary>
    public sealed class LoginSuccessEventHandler_Security : EventHandler<LoginSuccessEvent>
    {
        private readonly ISecurityRepository _securityRepository; // 安全仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LoginSuccessEventHandler_Security(
            ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoginSuccessEvent @event, CancellationToken cancellationToken)
        {
            await _securityRepository.UnsignAccount(@event.Account);
            await _securityRepository.UnsignAddress(@event.RemoteIp);
            await _securityRepository.WriteLog(nameof(SecurityLog.Online), JsonHelper.Serialize(new
            {
                @event.Account,
                @event.RemoteIp,
                @event.UserAgent,
                @event.Session
            }));
        }
    }
}
