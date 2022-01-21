using Kean.Domain.Basic.Commands;
using Kean.Domain.Basic.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Basic.CommandHandlers
{
    /// <summary>
    /// 设置角色菜单权限命令处理程序
    /// </summary>
    public sealed class SetMenuPermissionCommandHandler : CommandHandler<SetMenuPermissionCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SetMenuPermissionCommandHandler(
            ICommandBus commandBus,
            IRoleRepository roleRepository)
        {
            _commandBus = commandBus;
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
                }
                else
                {
                    await _roleRepository.SetMenuPermission(command.Id, command.Permission);
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
