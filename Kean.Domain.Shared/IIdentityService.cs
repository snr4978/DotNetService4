using System.Threading.Tasks;

namespace Kean.Domain.Shared
{
    /// <summary>
    /// 表示身份域共享服务
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <returns>密文</returns>
        Task<string> EncodePassword(string password);

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>会话</returns>
        Task<string> GetSession(string token);
    }
}
