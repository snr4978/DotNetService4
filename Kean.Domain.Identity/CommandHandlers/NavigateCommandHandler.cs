using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Models;
using Kean.Domain.Identity.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 导航命令处理程序
    /// </summary>
    public sealed class NavigateCommandHandler : CommandHandler<NavigateCommand>
    {
        private readonly ICommandBus _commandBus; // 消息总线
        private readonly ISessionRepository _sessionRepository; // 会话仓库
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public NavigateCommandHandler(
            ICommandBus commandBus,
            ISessionRepository sessionRepository,
            IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(NavigateCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                // 根据令牌计算会话索引
                var session = new Session(command.Token);
                // 密码初始化策略
                if (!await _sessionRepository.IsPasswordInitial(session))
                {
                    await _commandBus.Notify(nameof(command.Token), "用户密码初始化策略致导航异常中止", command.Token, nameof(ErrorCode.Precondition), cancellationToken);
                    return;
                }
                // 密码过期策略
                if (await _sessionRepository.IsPasswordExpired(session))
                {
                    await _commandBus.Notify(nameof(command.Token), "用户密码过期策略致导航异常中止", command.Token, nameof(ErrorCode.Expired), cancellationToken);
                    return;
                }
                // 权限验证
                var result = command.Ignore.Contains(command.Url) || await _sessionRepository.HasPermission(session, command.Url);
                if (!result)
                {
                    // 一次验证失败后，尝试重新同步持久化信息并再进行一次验证
                    var identity = await _sessionRepository.GetIdentity(session);
                    if (identity.HasValue)
                    {
                        result = (await _userRepository.MenuPermission(identity.Value)).Contains(command.Url);
                    }
                }
                // 结果
                if (!result)
                {
                    await _commandBus.Notify(nameof(command.Url), "没有 URL 的访问权限", command.Url, null, cancellationToken);
                }
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult, cancellationToken);
            }
        }
    }
}
