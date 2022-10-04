using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Repositories;
using Kean.Domain.Admin.SharedServices.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 重置密码命令处理程序
    /// </summary>
    public sealed class ResetPasswordCommandHandler : CommandHandler<ResetPasswordCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IUserRepository _userRepository; // 用户仓库
        private readonly IdentityProxy _identityProxy;// 身份域代理

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ResetPasswordCommandHandler(
            ICommandBus commandBus,
            IUserRepository userRepository,
            IdentityProxy identityProxy)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _identityProxy = identityProxy;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (!await _userRepository.IsExist(command.Id))
                {
                    await _commandBus.Notify(nameof(command.Id), "用户不存在", command.Id, nameof(ErrorCode.Gone),
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await _userRepository.ResetPassword(command.Id, s => _identityProxy.EncodePassword(s));
                }
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
