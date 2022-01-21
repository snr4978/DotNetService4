using Kean.Application.Command.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 应用启动过滤器
    /// </summary>
    public class StartupFilter : IStartupFilter
    {
        private readonly IServiceProvider _serviceProvider; // 服务对象

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.StartupFilter 类的新实例
        /// </summary>
        public StartupFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            using var scope = _serviceProvider.CreateScope();

            // 初始化缓存
            var appService = scope.ServiceProvider.GetService<IAppService>();
            appService.InitParam().Wait();
            appService.InitBlacklist().Wait();

            //清理会话
            var identityService = scope.ServiceProvider.GetService<IIdentityService>();
            identityService.Finalize().Wait();

            return next;
        }
    }
}
