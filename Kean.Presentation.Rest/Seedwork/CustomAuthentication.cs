using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 自定义身份认证程序
    /// </summary>
    public abstract class CustomAuthentication
    {
        /// <summary>
        /// 容器
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="context">Http 上下文</param>
        /// <returns>如果认证成功，返回 True；否则返回 False</returns>
        public abstract Task<bool> Authenticate(HttpContext context);
    }
}
