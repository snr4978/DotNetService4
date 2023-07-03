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
        Task LoadAll();

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="key">参数键</param>
        /// <returns>参数验证信息</returns>
        Task<string> GetValidation(string key);

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key">参数键</param>
        /// <returns>参数值</returns>
        Task<string> GetValue(string key);

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        Task SetValue(string key, string value);
    }
}
