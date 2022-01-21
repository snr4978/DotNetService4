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
    public sealed class ModifyPasswordCommandHandler : CommandHandler<ModifyPasswordCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ModifyPasswordCommandHandler(
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
        public override async Task Handle(ModifyPasswordCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (await _userRepository.VerifyPassword(command.Id, new Password(command.Current)))
                {
                    if (await _userRepository.ModifyPassword(command.Id, new Password(command.Replacement)))
                    {
                        await _commandBus.Trigger(_mapper.Map<ModifyPasswordSuccessEvent>(command), cancellationToken);
                    }
                    else
                    {
                        await _commandBus.Notify(nameof(command.Replacement), "新密码格式不正确", command.Replacement,
                            cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    await _commandBus.Notify(nameof(command.Current), "当前密码不正确", command.Current,
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
