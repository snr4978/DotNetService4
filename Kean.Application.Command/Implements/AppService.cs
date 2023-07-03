using Kean.Application.Command.Interfaces;
using Kean.Domain;
using Kean.Domain.App.Commands;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Command.Implements
{
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public class AppService : IAppService
    {
        private readonly ICommandBus _bus; // 命令总线
        private readonly INotification _notifications; // 总线通知

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AppService(
            ICommandBus bus,
            INotification notifications)
        {
            _bus = bus;
            _notifications = notifications;
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IAppService.InitParam() 方法
         */
        public async Task InitParam()
        {
            await _bus.Execute(new LoadParamCommand());
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IAppService.InitBlacklist() 方法
         */
        public async Task InitBlacklist()
        {
            await _bus.Execute(new LoadBlacklistCommand());
        }

        /*
         * 实现 Kean.Application.Command.Interfaces.IAppService.SetParam(string key, string value) 方法
         */
        public async Task<ViewModels.Failure> SetParam(string key, string value)
        {
            var command = new ConfigParamCommand { Key = key, Value = value };
            await _bus.Execute(command);
            return _notifications.FirstOrDefault();
        }
    }
}
