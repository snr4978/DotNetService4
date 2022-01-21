using Kean.Application.Command.Interfaces;
using Kean.Domain.Message.Sockets;
using Kean.Infrastructure.SignalR;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Hubs
{
    /// <summary>
    /// 消息集线器
    /// </summary>
    [Route("signalr/message")]
    public sealed class MessageHub : Hub, IOnlineSocket
    {
        private readonly IHubContext<MessageHub> _hub; // 集线器
        private readonly IMessageService _messageService; // 消息命令服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MessageHub(
            IHubContext<MessageHub> hub,
            IMessageService messageService)
        {
            _hub = hub;
            _messageService = messageService;
        }

        /*
         * 重写 Microsoft.AspNetCore.SignalR.Hub.OnConnectedAsync() 方法
         */
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await _messageService.Connect(Context.Features.Get<IHttpContextFeature>().HttpContext.Request.Query["access_token"].ToString(), Context.ConnectionId);
        }

        /*
         * 重写 Microsoft.AspNetCore.SignalR.Hub.OnDisconnectedAsync(Exception exception) 方法
         */
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await _messageService.Disconnect(Context.Features.Get<IHttpContextFeature>().HttpContext.Request.Query["access_token"].ToString(), Context.ConnectionId);
        }

        /*
         * 实现 Kean.Domain.Message.Sockets.IOnlineSocket.Notify(IEnumerable<string> connectionIds) 方法
         */
        public Task Notify(IEnumerable<string> connectionIds)
        {
            return _hub.Clients.Clients(connectionIds).SendAsync("notify");
        }
    }
}
