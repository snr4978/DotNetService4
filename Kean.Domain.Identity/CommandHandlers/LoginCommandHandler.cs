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
    /// 登录命令处理程序
    /// </summary>
    public sealed class LoginCommandHandler : CommandHandler<LoginCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly ISecurityRepository _securityRepository; // 安全仓库
        private readonly ISessionRepository _sessionRepository; // 会话仓库
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public LoginCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            ISecurityRepository securityRepository,
            ISessionRepository sessionRepository,
            IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _securityRepository = securityRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                // 身份认证
                var identity = await _userRepository.GetIdentity(command.Account, new Password(command.Password));
                if (identity == null)
                {
                    // 认证失败
                    await _commandBus.Trigger(_mapper.Map<LoginFailEvent>(command), cancellationToken);
                }
                else
                {
                    if (await _securityRepository.AccountIsFrozen(command.Account))
                    {
                        // 账号被冻结
                        Output(nameof(command.Token), string.Empty);
                    }
                    else
                    {
                        // 登录成功，创建会话
                        string token = new Token();
                        Output(nameof(command.Token), token);
                        var session = new Session(token);
                        await _sessionRepository.CreateSession(session, identity.Value);
                        var @event = _mapper.Map<LoginSuccessEvent>(command);
                        @event.Session = session;
                        await _commandBus.Trigger(@event, cancellationToken);
                    }
                }
            }
        }
    }
}
