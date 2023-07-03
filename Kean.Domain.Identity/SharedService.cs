using Kean.Domain.Identity.Models;
using Kean.Domain.Shared;
using System.Threading.Tasks;

namespace Kean.Domain.Identity
{
    /// <summary>
    /// 共享服务
    /// </summary>
    public class SharedService : IIdentityService
    {
        /*
         * 实现 Kean.Domain.Shared.IIdentityService.EncodePassword(string password) 方法
         */
        public Task<string> EncodePassword(string password) =>
             Task.FromResult<string>(new Password(password));

        /*
         * 实现 Kean.Domain.Shared.IIdentityService.GetSession(string token) 方法
         */
        public Task<string> GetSession(string token) =>
            Task.FromResult<string>(new Session(token));
    }
}
