using Kean.Domain.Identity.Commands;
using Kean.Domain.Identity.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.CommandHandlers
{
    /// <summary>
    /// 修改头像命令处理程序
    /// </summary>
    public sealed class ModifyAvatarCommandHandler : CommandHandler<ModifyAvatarCommand>
    {
        private readonly IUserRepository _userRepository; // 用户仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ModifyAvatarCommandHandler(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(ModifyAvatarCommand command, CancellationToken cancellationToken)
        {
            await _userRepository.ModifyAvatar(command.Id, command.Avatar);
        }
    }
}
