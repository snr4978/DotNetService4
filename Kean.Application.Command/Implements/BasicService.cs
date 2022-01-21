using AutoMapper;
using Kean.Application.Command.Interfaces;
using Kean.Application.Command.ViewModels;
using Kean.Domain;
using Kean.Domain.Basic.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Command.Implements
{
    /// <summary>
    /// 基础信息命令服务实现
    /// </summary>
    public class BasicService : IBasicService
    {
        private readonly ICommandBus _bus; // 命令总线
        private readonly IMapper _mapper; // 模型映射
        private readonly INotification _notifications; // 总线通知

        /// <summary>
        /// 依赖注入
        /// </summary>
        public BasicService(
            ICommandBus bus,
            IMapper mapper,
            INotification notifications)
        {
            _bus = bus;
            _mapper = mapper;
            _notifications = notifications;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.CreateRole(Role role) 方法
         */
        public async Task<(int Id, Failure Failure)> CreateRole(Role role)
        {
            var command = _mapper.Map<CreateRoleCommand>(role);
            await _bus.Execute(command);
            return (command.Id, _notifications.FirstOrDefault());
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.ModifyRole(Role role) 方法
         */
        public async Task<(bool Success, Failure Failure)> ModifyRole(Role role)
        {
            await _bus.Execute(_mapper.Map<ModifyRoleCommand>(role));
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.DeleteRole(IEnumerable<int> id) 方法
         */
        public async Task<IEnumerable<int>> DeleteRole(IEnumerable<int> id)
        {
            var command = new DeleteRoleCommand { Id = id };
            await _bus.Execute(command);
            return command.Id;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.SetRoleMenuPermission(int id, IEnumerable<int> permission) 方法
         */
        public async Task<(bool Success, Failure Failure)> SetRoleMenuPermission(int id, IEnumerable<int> permission)
        {
            await _bus.Execute(new SetMenuPermissionCommand
            {
                Id = id,
                Permission = permission
            });
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.CreateUser(User user) 方法
         */
        public async Task<(int Id, Failure Failure)> CreateUser(User user)
        {
            var command = _mapper.Map<CreateUserCommand>(user);
            await _bus.Execute(command);
            return (command.Id, _notifications.FirstOrDefault());
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.ModifyUser(User user) 方法
         */
        public async Task<(bool Success, Failure Failure)> ModifyUser(User user)
        {
            await _bus.Execute(_mapper.Map<ModifyUserCommand>(user));
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.DeleteUser(IEnumerable<int> id) 方法
         */
        public async Task<IEnumerable<int>> DeleteUser(IEnumerable<int> id)
        {
            var command = new DeleteUserCommand { Id = id };
            await _bus.Execute(command);
            return command.Id;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IBasicService.ResetPassword(int id) 方法
         */
        public async Task<(bool Success, Failure Failure)> ResetPassword(int id)
        {
            await _bus.Execute(new ResetPasswordCommand { Id = id });
            var failure = _notifications.FirstOrDefault();
            return (failure == null, failure);
        }
    }
}
