using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Models;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 断线命令处理程序
    /// </summary>
    public sealed class DisconnectCommandHandler : CommandHandler<DisconnectCommand>
    {
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DisconnectCommandHandler(
            ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DisconnectCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                await _sessionRepository.UnregisterConnection(new Session(command.Token), command.Id);
            }
        }
    }
}
