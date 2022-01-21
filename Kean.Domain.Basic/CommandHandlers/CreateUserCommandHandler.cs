using AutoMapper;
using Kean.Domain.Basic.Commands;
using Kean.Domain.Basic.Models;
using Kean.Domain.Basic.Repositories;
using Kean.Domain.Basic.SharedServices.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Basic.CommandHandlers
{
    /// <summary>
    /// 创建用户命令处理程序
    /// </summary>
    public sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库
        private readonly IdentityProxy _identityProxy;// 身份域代理

        /// <summary>
        /// 依赖注入
        /// </summary>
        public CreateUserCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            IUserRepository userRepository,
            IdentityProxy identityProxy)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _userRepository = userRepository;
            _identityProxy = identityProxy;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (await _userRepository.IsNameExist(command.Name, null))
                {
                    await _commandBus.Notify(nameof(command.Name), "用户名重复", command.Name, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                    return;
                }
                if (await _userRepository.IsAccountExist(command.Account, null))
                {
                    await _commandBus.Notify(nameof(command.Account), "用户账号重复", command.Account, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                    return;
                }
                Output(nameof(command.Id), await _userRepository.Create(_mapper.Map<User>(command), s => _identityProxy.EncodePassword(s)));
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
