using Kean.Domain.Identity.Enums;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using Kean.Domain.Identity.Sockets;
using Kean.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.EventHandlers
{
    /// <summary>
    /// 登录成功时，单点登录的实现
    /// </summary>
    public sealed class LoginSuccessEventHandler_Conflict : EventHandler<LoginSuccessEvent>
    {
        private readonly ISessionRepository _sessionRepository; // 会话仓库
        private readonly ISecurityRepository _securityRepository; // 安全仓库
        private readonly IOnlineSocket _onlineSocket; // 连接管道

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LoginSuccessEventHandler_Conflict(
            ISessionRepository sessionRepository,
            ISecurityRepository securityRepository,
            IOnlineSocket onlineSocket)
        {
            _sessionRepository = sessionRepository;
            _securityRepository = securityRepository;
            _onlineSocket = onlineSocket;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoginSuccessEvent @event, CancellationToken cancellationToken)
        {
            var conflicts = await _sessionRepository.GetConflicts(@event.Session);
            if (conflicts?.Any() == true)
            {
                var connections = new List<string>();
                foreach (var item in conflicts)
                {
                    var connection = await _sessionRepository.GetConnection(item);
                    if (connection != null)
                    {
                        connections.Add(connection);
                    }
                    await _sessionRepository.RemoveSession(item);
                    await _securityRepository.WriteLog(nameof(SecurityLog.Offline), JsonHelper.Serialize(new
                    {
                        Session = item,
                        Reason = nameof(OfflineReason.Conflict)
                    }));
                }
                await _onlineSocket.Offline(connections);
            }
        }
    }
}
