using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 指示 Controller 或 Action 需要按照指定的方式进行身份验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CustomAuthenticationAttribute : Attribute
    {
        /// <summary>
        /// 初始化 Kean.Presentation.Rest.CustomAuthenticationAttribute 类的新实例
        /// </summary>
        /// <param name="type">身份认证程序类型（该类型需要实现 CustomAuthentication 类）</param>
        public CustomAuthenticationAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// 身份认证程序类型
        /// 该类型需要实现 CustomAuthentication 类
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="context">Http 上下文</param>
        /// <param name="serviceProvider">容器</param>
        /// <returns>如果认证成功，返回 True；否则返回 False</returns>
        public Task<bool> Authenticate(HttpContext context, IServiceProvider serviceProvider)
        {
            var customAuthentication = Activator.CreateInstance(Type) as CustomAuthentication;
            customAuthentication.ServiceProvider= serviceProvider;
            return customAuthentication.Authenticate(context);
        }
    }
}
