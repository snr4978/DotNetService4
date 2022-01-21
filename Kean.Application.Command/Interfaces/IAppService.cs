using System.Threading.Tasks;

namespace Kean.Application.Command.Interfaces
{
    /// <summary>
    /// 表示应用程序服务
    /// </summary>
    public interface IAppService
    {
        /// <summary>
        /// 初始化系统参数
        /// </summary>
        Task InitParam();

        /// <summary>
        /// 初始化黑名单
        /// </summary>
        Task InitBlacklist();
    }
}
