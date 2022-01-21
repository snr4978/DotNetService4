using Kean.Domain.App.Commands;
using Kean.Domain.App.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.App.CommandHandlers
{
    /// <summary>
    /// 黑名单加载命令处理程序
    /// </summary>
    public sealed class LoadBlacklistCommandHandler : CommandHandler<LoadBlacklistCommand>
    {
        private readonly ISecurityRepository _securityRepository; // 安全性仓库

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadBlacklistCommandHandler(
            ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoadBlacklistCommand command, CancellationToken cancellationToken)
        {
            await _securityRepository.LoadBlacklist();
        }
    }
}
