using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 批量命令处理程序
    /// </summary>
    public sealed class BatchCommandHandler : CommandHandler<BatchCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly INotification _notification; // 总线通知

        /// <summary>
        /// 依赖注入
        /// </summary>
        public BatchCommandHandler(
            ICommandBus commandBus,
            INotification notification)
        {
            _commandBus = commandBus;
            _notification = notification;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(BatchCommand command, CancellationToken cancellationToken)
        {
            if (command.Commands?.Any() == true)
            {
                foreach (var item in command.Commands)
                {
                    await _commandBus.Execute(item, cancellationToken);
                    if (_notification.Any())
                    {
                        break;
                    }
                }
            }
        }
    }
}
