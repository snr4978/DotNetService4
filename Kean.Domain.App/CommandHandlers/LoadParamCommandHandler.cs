using Kean.Domain.App.Commands;
using Kean.Domain.App.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.App.CommandHandlers
{
    /// <summary>
    /// 系统参数加载命令处理程序
    /// </summary>
    public sealed class LoadParamCommandHandler : CommandHandler<LoadParamCommand>
    {
        private readonly IParamRepository _paramRepository; // 参数仓库

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadParamCommandHandler(
            IParamRepository paramRepository)
        {
            _paramRepository = paramRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(LoadParamCommand command, CancellationToken cancellationToken)
        {
            await _paramRepository.LoadParam();
        }
    }
}
