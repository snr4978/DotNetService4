using Kean.Application.Command.Interfaces;
using Kean.Domain.Identity.Sockets;
using Kean.Infrastructure.SignalR;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Hubs
{
    /// <summary>
    /// 在线集线器
    /// </summary>
    [Route("signalr/identity")]
    public sealed class IdentityHub : Hub, IOnlineSocket
    {
        private readonly IHubContext<IdentityHub> _hub; // 集线器
        private readonly IIdentityService _identityService; // 身份命令服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public IdentityHub(
            IHubContext<IdentityHub> hub,
            IIdentityService identityService)
        {
            _hub = hub;
            _identityService = identityService;
        }

        /*
         * 重写 Microsoft.AspNetCore.SignalR.Hub.OnConnectedAsync() 方法
         */
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await _identityService.Connect(Context.Features.Get<IHttpContextFeature>().HttpContext.Request.Query["access_token"].ToString(), Context.ConnectionId);
        }

        /*
         * 重写 Microsoft.AspNetCore.SignalR.Hub.OnDisconnectedAsync(Exception exception) 方法
         */
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await _identityService.Disconnect(Context.Features.Get<IHttpContextFeature>().HttpContext.Request.Query["access_token"].ToString(), Context.ConnectionId);
        }

        /*
         * 实现 Kean.Domain.Identity.Sockets.IOnlineSocket.Offline(IEnumerable<string> connectionIds) 方法
         */
        public Task Offline(IEnumerable<string> connectionIds)
        {
            return _hub.Clients.Clients(connectionIds).SendAsync("conflict");
        }
    }
}
