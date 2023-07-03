using AutoMapper;
using Kean.Application.Command.Interfaces;
using Kean.Application.Command.ViewModels;
using Kean.Domain;
using Kean.Domain.Identity.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Command.Implements
{
    /// <summary>
    /// 身份命令服务实现
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly ICommandBus _bus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly INotification _notifications; // 总线通知

        /// <summary>
        /// 依赖注入
        /// </summary>
        public IdentityService(
            ICommandBus bus,
            IMapper mapper,
            INotification notifications)
        {
            _bus = bus;
            _mapper = mapper;
            _notifications = notifications;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Login(User user) 方法
         */
        public async Task<string> Login(User user, string remoteIp, string userAgent)
        {
            var command = _mapper.Map<LoginCommand>(user);
            command.RemoteIp = remoteIp;
            command.UserAgent = userAgent;
            await _bus.Execute(command);
            return command.Token;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Connect(string token, string id) 方法
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
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Disconnect(string token, string id) 方法
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
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Logout(string session, string reason) 方法
         */
        public async Task Logout(string token, string reason)
        {
            await _bus.Execute(new LogoutCommand
            {
                Token = token,
                Reason = reason
            });
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Authenticate(string token) 方法
         */
        public async Task<int?> Authenticate(string token)
        {
            var command = new AuthenticateCommand { Token = token };
            await _bus.Execute(command);
            return command.Identity;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Navigate(string token, string url) 方法
         */
        public async Task<(IEnumerable<string> Permission, Failure Failure)> Navigate(string token, string url, params string[] ignore)
        {
            var command = new NavigateCommand
            {
                Token = token,
                Url = url,
                Ignore = ignore
            };
            await _bus.Execute(command);
            var failure = _notifications.FirstOrDefault();
            return (command.Permission, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.InitializePassword(Password password) 方法
         */
        public async Task<(bool Success, Failure Failure)> InitializePassword(Password password)
        {
            await _bus.Execute(_mapper.Map<InitializePasswordCommand>(password));
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.ModifyPassword(Password password) 方法
         */
        public async Task<(bool Success, Failure Failure)> ModifyPassword(Password password)
        {
            await _bus.Execute(_mapper.Map<ModifyPasswordCommand>(password));
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.ModifyAvatar(User user) 方法
         */
        public async Task<(bool Success, Failure Failure)> ModifyAvatar(User user)
        {
            await _bus.Execute(_mapper.Map<ModifyAvatarCommand>(user));
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IIdentityService.Finalize() 方法
         */
        public async Task Finalize()
        {
            await _bus.Execute(new FinalizeCommand());
        }
    }
}
