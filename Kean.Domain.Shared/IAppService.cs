using System.Threading.Tasks;

namespace Kean.Domain.Shared
{
    /// <summary>
    /// 表示应用域共享服务
    /// </summary>
    public interface IAppService
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key">参数键</param>
        /// <returns>参数值</returns>
        Task<string> GetParam(string key);
    }
}
