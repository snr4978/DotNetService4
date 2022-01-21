using System.Threading.Tasks;

namespace Kean.Domain.App.Repositories
{
    /// <summary>
    /// 表示安全性仓库
    /// </summary>
    public interface ISecurityRepository
    {
        /// <summary>
        /// 加载黑名单
        /// </summary>
        Task LoadBlacklist();
    }
}
