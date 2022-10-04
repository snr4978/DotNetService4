using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 删除用户命令处理程序
    /// </summary>
    public sealed class DeleteUserCommandHandler : CommandHandler<DeleteUserCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteUserCommandHandler(
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
        public override async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                List<int> result = new();
                foreach (var item in command.Id)
                {
                    await _userRepository.Delete(item);
                    result.Add(item);
                }
                command.Id = result;
                await _commandBus.Trigger(_mapper.Map<DeleteUserSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
