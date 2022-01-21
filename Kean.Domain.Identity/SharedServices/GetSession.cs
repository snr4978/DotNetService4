using Kean.Domain.Identity.Models;
using System.Threading.Tasks;

namespace Kean.Domain.Identity.SharedServices
{
    /// <summary>
    /// 获取会话
    /// </summary>
    public sealed class GetSession
    {
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>会话</returns>
        public Task<string> Handler(string token) => Task.FromResult<string>(new Session(token));
    }
}
