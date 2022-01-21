using Kean.Domain.Message.Commands;
using Kean.Domain.Message.Repositories;
using Kean.Domain.Message.SharedServices.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Message.CommandHandlers
{
    /// <summary>
    /// 断线命令处理程序
    /// </summary>
    public sealed class DisconnectCommandHandler : CommandHandler<DisconnectCommand>
    {
        private readonly IMessageRepository _messageRepository; // 消息仓库
        private readonly IdentityProxy _identityProxy;// 身份域代理

        /// <summary>
        /// 依赖注入
        /// </summary>
        public DisconnectCommandHandler(
            IMessageRepository messageRepository,
            IdentityProxy identityProxy)
        {
            _messageRepository = messageRepository;
            _identityProxy = identityProxy;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(DisconnectCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                var session = await _identityProxy.GetSession(command.Token);
                await _messageRepository.UnregisterConnection(session, command.Id);
            }
        }
    }
}
