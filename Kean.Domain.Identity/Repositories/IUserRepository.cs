using Kean.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.Repositories
{
    /// <summary>
    /// 表示用户仓库
    /// 包含对于用户的基本操作
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>用户 ID</returns>
        Task<int?> GetIdentity(string account, Password password);

        /// <summary>
        /// 密码是否初始化
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <returns>如果未设置密码初始化策略，则为 null</returns>
        Task<bool?> PasswordInitial(int id);

        /// <summary>
        /// 密码过期时间
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <returns>如果未设置密码过期策略，则为 null</returns>
        Task<DateTime?> PasswordExpired(int id);

        /// <summary>
        /// 校验当前密码
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <param name="password">当前密码</param>
        /// <returns>校验结果</returns>
        Task<bool> VerifyPassword(int id, Password password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <param name="password">密码</param>
        /// <returns>修改结果</returns>
        Task<bool> ModifyPassword(int id, Password password);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <param name="avatar">头像</param>
        /// <returns>修改结果</returns>
        Task<bool> ModifyAvatar(int id, string avatar);

        /// <summary>
        /// 菜单权限
        /// </summary>
        /// <param name="id">身份标识</param>
        /// <returns>权限内的菜单 URL</returns>
        Task<IEnumerable<string>> MenuPermission(int id);
    }
}
