using Kean.Application.Query.ViewModels;
using System.Threading.Tasks;

namespace Kean.Application.Query.Interfaces
{
    /// <summary>
    /// 表示应用程序查询服务
    /// </summary>
    public interface IAppService
    {
        /// <summary>
        /// 在黑名单中按地址查询结果
        /// </summary>
        /// <param name="address">地址</param>
        /// <returns>查询结果</returns>
        Task<Blacklist> GetBlacklist(string address);

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        Task<string> GetParam(string key);
    }
}
