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
    /// 修改密码命令处理程序
    /// </summary>
    public sealed class InitializePasswordCommandHandler : CommandHandler<InitializePasswordCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public InitializePasswordCommandHandler(
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
        public override async Task Handle(InitializePasswordCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (await _userRepository.PasswordInitial(command.Id) == false)
                {
                    if (await _userRepository.ModifyPassword(command.Id, new Password(command.Replacement)))
                    {
                        await _commandBus.Trigger(_mapper.Map<InitializePasswordSuccessEvent>(command), cancellationToken);
                    }
                    else
                    {
                        await _commandBus.Notify(nameof(command.Replacement), "密码格式不正确", command.Replacement,
                            cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await _commandBus.Notify(nameof(command.Id), "密码已经初始化", command.Id,
                        cancellationToken: cancellationToken);
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
