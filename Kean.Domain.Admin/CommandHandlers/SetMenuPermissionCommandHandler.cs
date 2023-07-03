using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 设置角色菜单权限命令处理程序
    /// </summary>
    public sealed class SetMenuPermissionCommandHandler : CommandHandler<SetMenuPermissionCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SetMenuPermissionCommandHandler(
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
        public override async Task Handle(SetMenuPermissionCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (!await _roleRepository.IsExist(command.Id))
                {
                    await _commandBus.Notify(nameof(command.Id), "角色不存在", command.Id, nameof(ErrorCode.Gone),
                        cancellationToken: cancellationToken);
                    return;
                }
                await _roleRepository.SetMenuPermission(command.Id, command.Permission);
                await _commandBus.Trigger(_mapper.Map<SetMenuPermissionSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
