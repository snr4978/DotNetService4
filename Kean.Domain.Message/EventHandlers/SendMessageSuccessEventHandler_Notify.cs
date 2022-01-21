using Kean.Domain.Message.Events;
using Kean.Domain.Message.Repositories;
using Kean.Domain.Message.Sockets;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.Message.EventHandlers
{
    /// <summary>
    /// 发送消息成功时，通知目标客户端
    /// </summary>
    public sealed class SendMessageSuccessEventHandler_Notify : EventHandler<SendMessageSuccessEvent>
    {
        private readonly IMessageRepository _messageRepository; // 连接仓库
        private readonly IOnlineSocket _onlineSocket; // 连接管道

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SendMessageSuccessEventHandler_Notify(
            IMessageRepository messageRepository,
            IOnlineSocket onlineSocket)
        {
            _messageRepository = messageRepository;
            _onlineSocket = onlineSocket;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(SendMessageSuccessEvent @event, CancellationToken cancellationToken)
        {
            var connectionIds = @event.Targets
                .Select(i => _messageRepository.GetConnections(i))
                .SelectMany(t => t.Result);
            if (connectionIds.Any())
            {
                await _onlineSocket.Notify(connectionIds);
            }
        }
    }
}
