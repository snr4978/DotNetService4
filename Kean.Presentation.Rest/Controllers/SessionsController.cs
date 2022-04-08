using Kean.Application.Command.Interfaces;
using Kean.Application.Command.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Controllers
{
    /// <summary>
    /// 会话服务
    /// </summary>
    [ApiController, Route("api/sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly IIdentityService _identityCommandService; // 身份命令服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SessionsController(
            IIdentityService identityCommandService)
        {
            _identityCommandService = identityCommandService;
        }

        /// <summary>
        /// 创建资源（登录）
        /// </summary>
        /// <response code="201">成功</response>
        /// <response code="401">账号或密码错误</response>
        /// <response code="423">账号被冻结</response>
        [HttpPost, Anonymous]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(423)]
        public async Task<IActionResult> Login(User user, [FromMiddleware] string ip, [FromMiddleware] string ua)
        {
            var token = await _identityCommandService.Login(user, ip, ua);
            return token switch
            {
                null => StatusCode(401),
                "" => StatusCode(423),
                _ => StatusCode(201, new { token })
            };
        }

        /// <summary>
        /// 删除资源（注销）
        /// </summary>
        /// <response code="204">成功</response>
        [HttpDelete, Anonymous]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout(string token, string reason)
        {
            await _identityCommandService.Logout(token, reason);
            return StatusCode(204);
        }
    }
}
