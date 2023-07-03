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

        /// <summary>
        /// 设置系统参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>失败信息</returns>
        Task<ViewModels.Failure> SetParam(string key, string value);
    }
}
