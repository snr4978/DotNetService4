using AutoMapper;
using Kean.Domain.Message.Commands;
using Kean.Domain.Message.Events;
using Kean.Domain.Message.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Message.CommandHandlers
{
    /// <summary>
    /// 发送命令处理程序
    /// </summary>
    public sealed class SendMessageCommandHandler : CommandHandler<SendMessageCommand>
    {
        private readonly ICommandBus _commandBus; // 消息总线
        private readonly IMapper _mapper; // 模型映射
        private readonly IMessageRepository _messageRepository; // 消息仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SendMessageCommandHandler(
            ICommandBus commandBus,
            IMapper mapper,
            IMessageRepository messageRepository)
        {
            _commandBus = commandBus;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                var now = DateTime.Now;
                foreach (var target in command.Targets)
                {
                    if (!await _messageRepository.SendMessage(command.Subject, command.Content, command.Source, target, now))
                    {
                        await _commandBus.Notify(nameof(command.Targets), "无法发送消息", target,
                            cancellationToken: cancellationToken);
                        return;
                    }
                }
                await _commandBus.Trigger(_mapper.Map<SendMessageSuccessEvent>(command), cancellationToken);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
