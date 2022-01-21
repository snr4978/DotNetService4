using System.Threading.Tasks;

namespace Kean.Domain.Message.SharedServices.Proxies
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
        /// 获取会话
        /// </summary>
        /// <param name="token">令牌</param>
        public Task<string> GetSession(string token) => _domain["Identity"].SharedService["GetSession"].Invoke<string>(token);
    }
}
