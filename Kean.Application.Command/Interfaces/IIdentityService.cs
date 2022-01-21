using Kean.Application.Command.ViewModels;
using System.Threading.Tasks;

namespace Kean.Application.Command.Interfaces
{
    /// <summary>
    /// 表示身份命令服务
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户视图</param>
        /// <param name="remoteIp">远程地址</param>
        /// <param name="userAgent">UA 信息</param>
        /// <returns>令牌。当用户身份验证成功时，生成 Guid 字符串作为令牌，如果令牌为 string.Empty，则表示用户被禁止操作（冻结）；当用户身份验证失败时，返回 null</returns>
        Task<string> Login(User user, string remoteIp, string userAgent);

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="id">连接 ID</param>
        Task Connect(string token, string id);

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="id">连接 ID</param>
        Task Disconnect(string token, string id);

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="reason">原因</param>
        Task Logout(string token, string reason);

        /// <summary>
        /// 根据客户端令牌进行会话状态验证
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>身份标识。验证失败时为 null</returns>
        Task<int?> Authenticate(string token);

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="url">URL</param>
        /// <param name="ignore">忽略项</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> Navigate(string token, string url, params string[] ignore);

        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="password">修改视图</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> InitializePassword(Password password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">修改视图</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> ModifyPassword(Password password);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="user">用户视图</param>
        /// <returns>操作结果及失败信息</returns>
        Task<(bool Success, Failure Failure)> ModifyAvatar(User user);

        /// <summary>
        /// 终结非法会话
        /// </summary>
        Task Finalize();
    }
}
