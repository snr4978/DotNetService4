using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Models;
using Kean.Domain.Admin.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 修改用户命令处理程序
    /// </summary>
    public sealed class ModifyUserCommandHandler : CommandHandler<ModifyUserCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ModifyUserCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(ModifyUserCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (!await _userRepository.IsExist(command.Id))
                {
                    await _commandBus.Notify(nameof(command.Id), "用户不存在", command.Id, nameof(ErrorCode.Gone),
                        cancellationToken: cancellationToken);
                    return;
                }
                if (await _userRepository.IsNameExist(command.Name, command.Id))
                {
                    await _commandBus.Notify(nameof(command.Name), "用户名重复", command.Name, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                    return;
                }
                if (await _userRepository.IsAccountExist(command.Account, command.Id))
                {
                    await _commandBus.Notify(nameof(command.Account), "用户账号重复", command.Account, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                    return;
                }
                await _userRepository.Modify(_mapper.Map<User>(command));
                await _commandBus.Trigger(_mapper.Map<ModifyUserSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
