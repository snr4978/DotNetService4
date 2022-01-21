using System.Threading.Tasks;

namespace Kean.Domain.Identity.Repositories
{
    /// <summary>
    /// 表示安全性仓库
    /// </summary>
    public interface ISecurityRepository
    {
        /// <summary>
        /// 标记地址安全性
        /// </summary>
        /// <param name="address">地址</param>
        Task SignAddress(string address);

        /// <summary>
        /// 取消地址安全性标记
        /// </summary>
        /// <param name="address">地址</param>
        Task UnsignAddress(string address);

        /// <summary>
        /// 标记账号安全性
        /// </summary>
        /// <param name="account">账号</param>
        Task SignAccount(string account);

        /// <summary>
        /// 取消账号安全性标记
        /// </summary>
        /// <param name="account">账号</param>
        Task UnsignAccount(string account);

        /// <summary>
        /// 账号是否冻结
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>如果冻结，为 true；否则为 false</returns>
        Task<bool> AccountIsFrozen(string account);

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="content">内容</param>
        Task WriteLog(string tag, string content);
    }
}
