using Kean.Application.Command.Interfaces;
using Kean.Infrastructure.Soap;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 身份验证中间件
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next; // 管道

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.AuthenticationMiddleware 类的新实例
        /// </summary>
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider, IIdentityService service)
        {
            // 令牌
            var token = context.Request.Headers["Token"];
            if (token.Count > 0)
            {
                context.Items["token"] = token.ToString();
            }
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint == null)
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                // 自定义身份验证
                var authentication = endpoint.Metadata.GetMetadata<CustomAuthenticationAttribute>();
                if (authentication == null || await authentication.Authenticate(context, serviceProvider))
                {
                    // 对未标记 Anonymous 特性的 Action 以及非 SignalR 进行内置身份验证
                    if (endpoint.Metadata.GetMetadata<AnonymousAttribute>() != null
                        || endpoint.Metadata.GetMetadata<HubMetadata>() != null
                        || endpoint.Metadata.GetMetadata<SoapMetadata>() != null)
                    {
                        await _next(context);
                    }
                    else
                    {
                        if (token.Count == 0)
                        {
                            context.Response.StatusCode = 401;
                        }
                        else
                        {
                            var session = await service.Authenticate(token);
                            if (session.HasValue)
                            {
                                context.Items["session"] = session.Value;
                                await _next(context);
                            }
                            else
                            {
                                context.Response.StatusCode = 401;
                            }
                        }
                    }
                }
            }
        }
    }
}
