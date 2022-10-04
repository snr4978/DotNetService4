using System.Threading.Tasks;

namespace Kean.Domain.Admin.SharedServices.Proxies
{
    /// <summary>
    /// 身份域代理
    /// </summary>
    public sealed class IdentityProxy
    {
        private readonly IDomain _domain; // 域

        /// <summary>
        /// 依赖注入
        /// </summary>
        public IdentityProxy(IDomain domain) => _domain = domain;

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password">明文密码</param>
        public Task<string> EncodePassword(string password) => _domain["Identity"].SharedService["EncodePassword"].Invoke<string>(password);
    }
}
