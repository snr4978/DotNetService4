using System.Threading.Tasks;

namespace Kean.Domain.App.Repositories
{
    /// <summary>
    /// 表示参数仓库
    /// </summary>
    public interface IParamRepository
    {
        /// <summary>
        /// 加载全部参数
        /// </summary>
        Task LoadParam();
    }
}
