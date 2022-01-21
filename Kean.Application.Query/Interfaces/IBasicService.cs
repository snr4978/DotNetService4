using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Query.Interfaces
{
    /// <summary>
    /// 表示基础信息查询服务
    /// </summary>
    public interface IBasicService
    {
        /// <summary>
        /// 获取角色数量
        /// </summary>
        /// <param name="name">角色名</param>
        /// <returns>结果</returns>
        Task<int> GetRoleCount(string name);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="sort">排序</param>
        /// <param name="offset">偏移</param>
        /// <param name="limit">限制</param>
        /// <returns>结果视图</returns>
        Task<IEnumerable<Role>> GetRoleList(string name, string sort, int? offset, int? limit);

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id">角色 ID</param>
        /// <returns>权限信息</returns>
        Task<(Tree<Menu> Menu, IEnumerable<int> Permission)> GetRoleMenuPermission(int id);

        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="account">账号</param>
        /// <param name="role">角色</param>
        /// <returns>结果</returns>
        Task<int> GetUserCount(string name, string account, int? role);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="account">账号</param>
        /// <param name="role">角色</param>
        /// <param name="sort">排序</param>
        /// <param name="offset">偏移</param>
        /// <param name="limit">限制</param>
        /// <returns>结果视图</returns>
        Task<IEnumerable<User>> GetUserList(string name, string account, int? role, string sort, int? offset, int? limit);
    }
}
