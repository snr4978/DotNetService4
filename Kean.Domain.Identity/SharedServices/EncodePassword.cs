using Kean.Domain.Identity.Models;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.SharedServices
{
    /// <summary>
    /// 密码加密
    /// </summary>
    public sealed class EncodePassword
    {
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <returns>密文</returns>
        public Task<string> Handler(string password) => Task.FromResult<string>(new Password(password));
    }
}
