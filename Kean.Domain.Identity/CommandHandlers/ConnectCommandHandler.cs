using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Models;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 连线命令处理程序
    /// </summary>
    public sealed class ConnectCommandHandler : CommandHandler<ConnectCommand>
    {
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ConnectCommandHandler(
            ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(ConnectCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                await _sessionRepository.RegisterConnection(new Session(command.Token), command.Id);
            }
        }
    }
}
