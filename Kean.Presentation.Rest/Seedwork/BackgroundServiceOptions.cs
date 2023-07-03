using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 后台任务选项
    /// </summary>
    public sealed class BackgroundServiceOptions
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.BackgroundServiceOptions 的新实例
        /// </summary>
        /// <param name="services">服务描述符</param>
        public BackgroundServiceOptions(IServiceCollection services) => 
            _services = services;

        /// <summary>
        /// 寄宿后台任务
        /// </summary>
        /// <typeparam name="T">后台任务类型</typeparam>
        public void Host<T>() where T : BackgroundService => 
            _services.AddScoped<T>().AddHostedService<BackgroundService<T>>();
    }
}
