using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Models;
using Kean.Domain.Admin.Repositories;
using Kean.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 创建用户命令处理程序
    /// </summary>
    public sealed class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库
        private readonly IIdentityService _identityService;// 身份域共享服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public CreateUserCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            IUserRepository userRepository,
            IIdentityService identityService)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _userRepository = userRepository;
            _identityService = identityService;
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
                Output(nameof(command.Id), await _userRepository.Create(_mapper.Map<User>(command), s => _identityService.EncodePassword(s)));
                await _commandBus.Trigger(_mapper.Map<CreateUserSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
