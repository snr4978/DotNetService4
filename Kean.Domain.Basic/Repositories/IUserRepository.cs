using Kean.Domain.Basic.Models;
using System;
using System.Threading.Tasks;

namespace Kean.Domain.Basic.Repositories
{
    /// <summary>
    /// 表示用户仓库
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns>如果存在为 true，否则为 false</returns>
        Task<bool> IsExist(int id);

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="igrone">忽略的用户 ID</param>
        /// <returns>如果存在为 true，否则为 false</returns>
        Task<bool> IsNameExist(string name, int? igrone);

        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="igrone">忽略的用户 ID</param>
        /// <returns>如果存在为 true，否则为 false</returns>
        Task<bool> IsAccountExist(string account, int? igrone);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="encode">加密方法</param>
        /// <returns>创建成功返回用户 ID</returns>
        Task<int> Create(User user, Func<string, Task<string>> encode);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        Task Modify(User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户 ID</param>
        Task Delete(int id);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <param name="encode">加密方法</param>
        Task ResetPassword(int id, Func<string, Task<string>> encode);

        /// <summary>
        /// 清理会话及缓存信息
        /// </summary>
        /// <param name="id">用户 ID</param>
        Task ClearSession(int id);
    }
}
