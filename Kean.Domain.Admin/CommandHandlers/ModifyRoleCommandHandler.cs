﻿using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Models;
using Kean.Domain.Admin.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 修改角色命令处理程序
    /// </summary>
    public sealed class ModifyRoleCommandHandler : CommandHandler<ModifyRoleCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IRoleRepository _roleRepository; // 角色仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ModifyRoleCommandHandler(
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
        public override async Task Handle(ModifyRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (!await _roleRepository.IsExist(command.Id))
                {
                    await _commandBus.Notify(nameof(command.Id), "角色不存在", command.Id, nameof(ErrorCode.Gone),
                        cancellationToken: cancellationToken);
                    return;
                }
                if (await _roleRepository.IsExist(command.Name, command.Id))
                {
                    await _commandBus.Notify(nameof(command.Name), "角色名重复", command.Name, nameof(ErrorCode.Conflict),
                        cancellationToken: cancellationToken);
                    return;
                }
                await _roleRepository.Modify(_mapper.Map<Role>(command));
                await _commandBus.Trigger(_mapper.Map<ModifyRoleSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
