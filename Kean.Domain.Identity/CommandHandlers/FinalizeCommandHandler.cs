using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 终结命令处理程序
    /// </summary>
    public sealed class FinalizeCommandHandler : CommandHandler<FinalizeCommand>
    {
        private readonly ICommandBus _commandBus; // 消息总线
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public FinalizeCommandHandler(
            ICommandBus commandBus,
            ISessionRepository sessionRepository)
        {
            _commandBus = commandBus;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(FinalizeCommand command, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.LoadKeys();
            foreach (var item in session)
            {
                if (await _sessionRepository.IsTimeout(item))
                {
                    await _sessionRepository.RemoveSession(item);
                }
            }
            await _commandBus.Trigger(new FinalizeSuccessEvent { Session = session }, cancellationToken);
        }
    }
}
