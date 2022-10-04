using Kean.Application.Command.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Application.Command.Interfaces
{
    /// <summary>
    /// 表示基础信息命令服务
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="role">角色视图</param>
        /// <returns>分配的 ID 及失败信息</returns>
        Task<(int Id, Failure Failure)> CreateRole(Role role);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色视图</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> ModifyRole(Role role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色 ID</param>
        /// <returns>成功删除的 ID</returns>
        Task<IEnumerable<int>> DeleteRole(IEnumerable<int> id);

        /// <summary>
        /// 设置角色菜单权限
        /// </summary>
        /// <param name="id">角色 ID</param>
        /// <param name="permission">菜单权限</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> SetRoleMenuPermission(int id, IEnumerable<int> permission);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户视图</param>
        /// <returns>分配的 ID 及失败信息</returns>
        Task<(int Id, Failure Failure)> CreateUser(User user);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户视图</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> ModifyUser(User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns>成功删除的 ID</returns>
        Task<IEnumerable<int>> DeleteUser(IEnumerable<int> id);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">用户 ID</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> ResetPassword(int id);
    }
}
