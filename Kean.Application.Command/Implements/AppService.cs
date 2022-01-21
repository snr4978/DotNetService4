using Kean.Application.Command.Interfaces;
using Kean.Domain;
using Kean.Domain.App.Commands;
using System.Threading.Tasks;

namespace Kean.Application.Command.Implements
{
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public class AppService : IAppService
    {
        private readonly ICommandBus _bus; // 命令总线

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AppService(
            ICommandBus bus)
        {
            _bus = bus;
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
    }
}
