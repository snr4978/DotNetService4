using AutoMapper;
using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Models;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 注销命令处理程序
    /// </summary>
    public sealed class LogoutCommandHandler : CommandHandler<LogoutCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LogoutCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            ISessionRepository sessionRepository)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            if (command.Token != null)
            {
                var session = new Session(command.Token);
                await _sessionRepository.RemoveSession(session);
                var @event = _mapper.Map<LogoutSuccessEvent>(command);
                @event.Session = session;
                await _commandBus.Trigger(@event, cancellationToken);
            }
        }
    }
}
