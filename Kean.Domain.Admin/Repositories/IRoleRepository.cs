using Kean.Domain.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Admin.Repositories
{
    /// <summary>
    /// 表示角色仓库
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="id">角色 ID</param>
        /// <returns>如果存在为 true，否则为 false</returns>
        Task<bool> IsExist(int id);

        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="name">角色名</param>
        /// <param name="igrone">忽略的角色 ID</param>
        /// <returns>如果存在为 true，否则为 false</returns>
        Task<bool> IsExist(string name, int? igrone);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>创建成功返回角色 ID</returns>
        Task<int> Create(Role role);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色</param>
        Task Modify(Role role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色 ID</param>
        Task Delete(int id);

        /// <summary>
        /// 设置角色菜单权限
        /// </summary>
        /// <param name="id">角色 ID</param>
        /// <param name="permission">菜单权限 ID</param>
        Task SetMenuPermission(int id, IEnumerable<int> permission);

        /// <summary>
        /// 清除角色菜单权限
        /// </summary>
        /// <param name="id">角色 ID</param>
        Task ClearMenuPermission(int id);

        /// <summary>
        /// 清除用户角色
        /// </summary>
        /// <param name="id">角色 ID</param>
        Task ClearUserRole(int id);
    }
}
