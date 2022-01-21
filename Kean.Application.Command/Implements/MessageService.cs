using AutoMapper;
using Kean.Application.Command.Interfaces;
using Kean.Application.Command.ViewModels;
using Kean.Domain;
using Kean.Domain.Message.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Command.Implements
{
    /// <summary>
    /// 消息服务
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly ICommandBus _bus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly INotification _notifications; // 总线通知

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MessageService(
            ICommandBus bus,
            IMapper mapper,
            INotification notifications)
        {
            _bus = bus;
            _mapper = mapper;
            _notifications = notifications;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IMessageService.Connect(string token, string id) 方法
         */
        public async Task Connect(string token, string id)
        {
            await _bus.Execute(new ConnectCommand
            {
                Token = token,
                Id = id
            });
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IMessageService.Disconnect(string token, string id) 方法
         */
        public async Task Disconnect(string token, string id)
        {
            await _bus.Execute(new DisconnectCommand
            {
                Token = token,
                Id = id
            });
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IMessageService.SendMessage(Message message, params int[] targets) 方法
         */
        public async Task<bool> SendMessage(Message message, params int[] targets)
        {
            var command = _mapper.Map<SendMessageCommand>(message);
            command.Targets = targets;
            await _bus.Execute(command);
            return !_notifications.Any();
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IMessageService.MarkMessage(int userId, IEnumerable<int> messageId, bool flag) 方法
         */
        public async Task<IEnumerable<int>> MarkMessage(int userId, IEnumerable<int> messageId, bool flag)
        {
            var command = new MarkMessageCommand
            {
                UserId = userId,
                MessageId = messageId,
                Flag = flag
            };
            await _bus.Execute(command);
            return command.MessageId;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IMessageService.DeleteMessage(int userId, IEnumerable<int> messageId) 方法
         */
        public async Task<IEnumerable<int>> DeleteMessage(int userId, IEnumerable<int> messageId)
        {
            var command = new DeleteMessageCommand
            {
                UserId = userId,
                MessageId = messageId
            };
            await _bus.Execute(command);
            return command.MessageId;
        }
    }
}
