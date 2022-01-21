using Kean.Application.Command.ViewModels;
using Kean.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kean.Presentation.Rest.Controllers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [ApiController, Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly Application.Command.Interfaces.IBasicService _basicCommandService; // 身份命令服务
        private readonly Application.Query.Interfaces.IBasicService _basicQueryService; // 身份查询服务
        private readonly Application.Command.Interfaces.IIdentityService _identityCommandService; // 身份命令服务
        private readonly Application.Query.Interfaces.IIdentityService _identityQueryService; // 身份查询服务
        private readonly Application.Command.Interfaces.IMessageService _messageCommandService; // 消息命令服务
        private readonly Application.Query.Interfaces.IMessageService _messageQueryService; // 消息查询服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public UsersController(
            Application.Command.Interfaces.IBasicService basicCommandService,
            Application.Query.Interfaces.IBasicService basicQueryService,
            Application.Command.Interfaces.IIdentityService identityCommandService,
            Application.Query.Interfaces.IIdentityService identityQueryService,
            Application.Command.Interfaces.IMessageService messageCommandService,
            Application.Query.Interfaces.IMessageService messageQueryService)
        {
            _basicCommandService = basicCommandService;
            _basicQueryService = basicQueryService;
            _identityCommandService = identityCommandService;
            _identityQueryService = identityQueryService;
            _messageCommandService = messageCommandService;
            _messageQueryService = messageQueryService;
        }

        #region 当前用户操作

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("current")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetProfile([FromMiddleware] int session)
        {
            var user = await _identityQueryService.GetUser(session);
            return StatusCode(200, user);
        }

        /// <summary>
        /// 修改当前用户的头像
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="422">图像内容错误</response>
        [HttpPut("current/profile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> ModifyAvatar(User user, [FromMiddleware] int session)
        {
            user.Id = session;
            var result = await _identityCommandService.ModifyAvatar(user);
            return result.Success ?
                StatusCode(200) :
                StatusCode(422, result.Failure.ErrorMessage);
        }

        /// <summary>
        /// 初始化当前用户的密码
        /// </summary>
        /// <response code="201">成功</response>
        /// <response code="405">密码已经初始化，不允许操作</response>
        /// <response code="422">密码格式错误</response>
        [HttpPost("current/password")]
        [ProducesResponseType(201)]
        [ProducesResponseType(405)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> InitializePassword(Password password, [FromMiddleware] int session)
        {
            password.Id = session;
            var result = await _identityCommandService.InitializePassword(password);
            return result switch
            {
                { Failure: { PropertyName: nameof(password.Id) } } => StatusCode(405),
                { Failure: { PropertyName: nameof(password.Replacement) } } => StatusCode(422),
                _ => StatusCode(201)
            };
        }

        /// <summary>
        /// 修改当前用户的密码
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="422">原密码错误或新密码格式错误</response>
        [HttpPut("current/password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> ModifyPassword(Password password, [FromMiddleware] int session)
        {
            password.Id = session;
            var result = await _identityCommandService.ModifyPassword(password);
            return result.Success ?
                StatusCode(200) :
                StatusCode(422, result.Failure.PropertyName.ToLower());
        }

        /// <summary>
        /// 获取当前用户菜单
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("current/routes")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMenu([FromMiddleware] int session)
        {
            var menu = await _identityQueryService.GetMenu(session);
            return StatusCode(200, menu);
        }

        /// <summary>
        /// 当前用户对指定路由的访问权限
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="403">没有权限</response>
        /// <response code="419">密码失效</response>
        /// <response code="428">密码未初始化</response>
        [HttpGet("current/routes/{url}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(419)]
        [ProducesResponseType(428)]
        public async Task<IActionResult> CheckPermission(string url, [FromMiddleware] string token)
        {
            var igrone = new string[]
            {
                HttpUtility.UrlEncode("/")
            };
            var result = await _identityCommandService.Navigate(token, url, igrone);
            return result switch
            {
                { Success: true } => StatusCode(200),
                { Failure: { ErrorCode: nameof(ErrorCode.Precondition) } } => StatusCode(428),
                { Failure: { ErrorCode: nameof(ErrorCode.Expired) } } => StatusCode(419),
                _ => StatusCode(403)
            };
        }

        /// <summary>
        /// 获取当前用户消息数
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("current/messages/count")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMessageCount(
            [FromQuery] string subject,
            [FromQuery] string source,
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end,
            [FromQuery] bool? flag,
            [FromMiddleware] int session)
        {
            var count = await _messageQueryService.GetCount(session, subject, source, start, end, flag);
            return StatusCode(200, count);
        }

        /// <summary>
        /// 获取当前用户消息
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("current/messages")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMessageList(
            [FromQuery] string subject,
            [FromQuery] string source,
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end,
            [FromQuery] bool? flag,
            [FromQuery] int? offset,
            [FromQuery] int? limit,
            [FromMiddleware] int session)
        {
            if (offset.HasValue)
            {
                var total = await _messageQueryService.GetCount(session, subject, source, start, end, flag);
                var items = await _messageQueryService.GetList(session, subject, source, start, end, flag, offset, limit);
                return StatusCode(200, new { total, items });
            }
            else
            {
                var messages = await _messageQueryService.GetList(session, subject, source, start, end, flag, offset, limit);
                return StatusCode(200, messages);
            }
        }

        /// <summary>
        /// 获取当前用户消息内容
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("current/messages/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMessageItem(int id, [FromMiddleware] int session)
        {
            var message = await _messageQueryService.GetItem(session, id);
            return StatusCode(200, message);
        }

        /// <summary>
        /// 批量处理消息
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="405">方法不支持</response>
        [HttpPost("current/messages/batch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(405)]
        public async Task<IActionResult> BatchMessage(Batch<Message> batch, [FromMiddleware] int session)
        {
            return batch.Method switch
            {
                BatchMethod.Update => StatusCode(200, await _messageCommandService.MarkMessage(session, batch.Data.Select(r => r.Id), batch.Data.First().Flag)),
                BatchMethod.Delete => StatusCode(200, await _messageCommandService.DeleteMessage(session, batch.Data.Select(r => r.Id))),
                _ => StatusCode(405)
            };
        }

        #endregion

        #region 用户管理操作

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetList(
            [FromQuery] string name,
            [FromQuery] string account,
            [FromQuery] int? role,
            [FromQuery] string sort,
            [FromQuery] int? offset,
            [FromQuery] int? limit)
        {
            var total = await _basicQueryService.GetUserCount(name, account, role);
            var items = await _basicQueryService.GetUserList(name, account, role, sort, offset, limit);
            return StatusCode(200, new { total, items });
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <response code="201">成功</response>
        /// <response code="409">用户已存在</response>
        /// <response code="422">请求内容错误</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create(User user)
        {
            var result = await _basicCommandService.CreateUser(user);
            return result switch
            {
                { Id: > 0 } => StatusCode(201, result.Id),
                { Failure: { ErrorCode: nameof(ErrorCode.Conflict) } } => StatusCode(409, result.Failure.PropertyName.ToLower()),
                _ => StatusCode(422)
            };
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="409">用户已存在</response>
        /// <response code="410">用户已删除</response>
        /// <response code="422">请求内容错误</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(410)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Modify(int id, User user)
        {
            user.Id = id;
            var result = await _basicCommandService.ModifyUser(user);
            return result switch
            {
                { Success: true } => StatusCode(200),
                { Failure: { ErrorCode: nameof(ErrorCode.Conflict) } } => StatusCode(409, result.Failure.PropertyName.ToLower()),
                { Failure: { ErrorCode: nameof(ErrorCode.Gone) } } => StatusCode(410),
                _ => StatusCode(422)
            };
        }

        /// <summary>
        /// 批量处理用户
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="405">方法不支持</response>
        [HttpPost("batch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(405)]
        public async Task<IActionResult> Batch(Batch<int> batch)
        {
            return batch.Method switch
            {
                BatchMethod.Delete => StatusCode(200, await _basicCommandService.DeleteUser(batch.Data)),
                _ => StatusCode(405)
            };
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <response code="204">成功</response>
        /// <response code="410">角色已删除</response>
        /// <response code="422">请求内容错误</response>
        [HttpDelete("{id}/password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(410)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var result = await _basicCommandService.ResetPassword(id);
            return result switch
            {
                { Success: true } => StatusCode(204),
                { Failure: { ErrorCode: nameof(ErrorCode.Gone) } } => StatusCode(410),
                _ => StatusCode(422)
            };
        }

        #endregion
    }
}
