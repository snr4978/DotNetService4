using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Infrastructure.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 登录失败时，处理安全策略
    /// </summary>
    public sealed class LoginFailEventHandler_Security : EventHandler<LoginFailEvent>
    {
        private readonly ISecurityRepository _securityRepository; // 安全仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LoginFailEventHandler_Security(
            ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoginFailEvent @event, CancellationToken cancellationToken)
        {
            await _securityRepository.SignAccount(@event.Account);
            await _securityRepository.SignAddress(@event.RemoteIp);
            await _securityRepository.WriteLog(nameof(SecurityLog.Online), JsonHelper.Serialize(new
            {
                @event.Account,
                @event.RemoteIp,
                @event.UserAgent
            }));
        }
    }
}
