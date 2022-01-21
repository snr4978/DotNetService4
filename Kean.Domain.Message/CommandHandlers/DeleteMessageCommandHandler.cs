using Kean.Domain.Message.Commands;
using Kean.Domain.Message.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Message.CommandHandlers
{
    /// <summary>
    /// 删除命令处理程序
    /// </summary>
    public sealed class DeleteMessageCommandHandler : CommandHandler<DeleteMessageCommand>
    {
        private readonly ICommandBus _commandBus; // 消息总线
        private readonly IMessageRepository _messageRepository; // 消息仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DeleteMessageCommandHandler(
            ICommandBus commandBus,
            IMessageRepository messageRepository)
        {
            _commandBus = commandBus;
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                List<int> result = new();
                foreach (var item in command.MessageId)
                {
                    await _messageRepository.DeleteMessage(command.UserId, item);
                    result.Add(item);
                }
                command.MessageId = result;
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
