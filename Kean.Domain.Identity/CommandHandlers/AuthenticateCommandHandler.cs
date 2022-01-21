using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Events;
using Kean.Domain.Identity.Models;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 身份验证命令处理程序
    /// </summary>
    public sealed class AuthenticateCommandHandler : CommandHandler<AuthenticateCommand>
    {
        private readonly ICommandBus _commandBus; // 消息总线
        private readonly ISessionRepository _sessionRepository; // 会话仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AuthenticateCommandHandler(
            ICommandBus commandBus,
            ISessionRepository sessionRepository)
        {
            _commandBus = commandBus;
            _sessionRepository = sessionRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(AuthenticateCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                // 根据令牌计算会话索引
                var session = new Session(command.Token);
                // 从缓存中读取会话
                var identity = await _sessionRepository.GetIdentity(session);
                if (identity.HasValue)
                {
                    if (await _sessionRepository.IsTimeout(session))
                    {
                        // 超时
                        await _commandBus.Trigger(new AuthenticateFailEvent { Session = session }, cancellationToken);
                    }
                    else
                    {
                        // 成功
                        Output(nameof(command.Identity), identity.Value);
                        await _commandBus.Trigger(new AuthenticateSuccessEvent { Session = session }, cancellationToken);
                    }
                }
                else
                {
                    // 会话不存在
                    await _commandBus.Trigger(new AuthenticateFailEvent { Session = session }, cancellationToken);
                }
            }
        }
    }
}
