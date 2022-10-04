using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Models;
using Kean.Domain.Admin.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 创建角色命令处理程序
    /// </summary>
    public sealed class CreateRoleCommandHandler : CommandHandler<CreateRoleCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public CreateRoleCommandHandler(
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
        public override async Task Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (await _roleRepository.IsExist(command.Name, null))
                {
                    await _commandBus.Notify(nameof(command.Name), "角色名重复", command.Name, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                }
                else
                {
                    Output(nameof(command.Id), await _roleRepository.Create(_mapper.Map<Role>(command)));
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
