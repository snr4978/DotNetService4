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
    /// 删除角色命令处理程序
    /// </summary>
    public sealed class DeleteRoleCommandHandler : CommandHandler<DeleteRoleCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteRoleCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            IRoleRepository roleRepository)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                List<int> result = new();
                foreach (var item in command.Id)
                {
                    await _roleRepository.Delete(item);
                    result.Add(item);
                }
                command.Id = result;
                await _commandBus.Trigger(_mapper.Map<DeleteRoleSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
