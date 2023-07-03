using AutoMapper;
using Kean.Domain.Admin.Commands;
using Kean.Domain.Admin.Events;
using Kean.Domain.Admin.Repositories;
using Kean.Domain.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.CommandHandlers
{
    /// <summary>
    /// 重置密码命令处理程序
    /// </summary>
    public sealed class ResetPasswordCommandHandler : CommandHandler<ResetPasswordCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IUserRepository _userRepository; // 用户仓库
        private readonly IIdentityService _identityService;// 身份域共享服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ResetPasswordCommandHandler(
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
        public override async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                if (!await _userRepository.IsExist(command.Id))
                {
                    await _commandBus.Notify(nameof(command.Id), "用户不存在", command.Id, nameof(ErrorCode.Gone),
                        cancellationToken: cancellationToken);
                    return;
                }
                await _userRepository.ResetPassword(command.Id, s => _identityService.EncodePassword(s));
                await _commandBus.Trigger(_mapper.Map<ResetPasswordSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
