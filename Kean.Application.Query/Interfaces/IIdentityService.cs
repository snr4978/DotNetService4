using Kean.Application.Query.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Query.Interfaces
{
    /// <summary>
    /// 表示身份查询服务
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// 根据 ID 查询指定用户
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns>用户视图</returns>
        Task<User> GetUser(int id);

        /// <summary>
        /// 查询指定用户的目录
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns>目录视图</returns>
        Task<IEnumerable<Menu>> GetMenu(int id);
    }
}
