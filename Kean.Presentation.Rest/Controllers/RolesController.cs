using Kean.Application.Command.ViewModels;
using Kean.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Controllers
{
    /// <summary>
    /// 角色服务
    /// </summary>
    [ApiController, Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly Application.Command.Interfaces.IBasicService _basicCommandService; // 基础信息命令服务
        private readonly Application.Query.Interfaces.IBasicService _basicQueryService; // 基础信息查询服务

        /// <summary>
        /// 依赖注入
        /// </summary>
        public RolesController(
            Application.Command.Interfaces.IBasicService basicCommandService,
            Application.Query.Interfaces.IBasicService basicQueryService)
        {
            _basicCommandService = basicCommandService;
            _basicQueryService = basicQueryService;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetList(
            [FromQuery] string name,
            [FromQuery] string sort,
            [FromQuery] int? offset,
            [FromQuery] int? limit)
        {
            var total = await _basicQueryService.GetRoleCount(name);
            var items = await _basicQueryService.GetRoleList(name, sort, offset, limit);
            return StatusCode(200, new { total, items });
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <response code="201">成功</response>
        /// <response code="409">角色已存在</response>
        /// <response code="422">请求内容错误</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create(Role role)
        {
            var result = await _basicCommandService.CreateRole(role);
            return result switch
            {
                { Id: > 0 } => StatusCode(201, result.Id),
                { Failure: { ErrorCode: nameof(ErrorCode.Conflict) } } => StatusCode(409),
                _ => StatusCode(422)
            };
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="409">角色已存在</response>
        /// <response code="410">角色已删除</response>
        /// <response code="422">请求内容错误</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        [ProducesResponseType(410)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Modify(int id, Role role)
        {
            role.Id = id;
            var result = await _basicCommandService.ModifyRole(role);
            return result switch
            {
                { Success: true } => StatusCode(200),
                { Failure: { ErrorCode: nameof(ErrorCode.Conflict) } } => StatusCode(409),
                { Failure: { ErrorCode: nameof(ErrorCode.Gone) } } => StatusCode(410),
                _ => StatusCode(422)
            };
        }

        /// <summary>
        /// 批量处理角色
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
                BatchMethod.Delete => StatusCode(200, await _basicCommandService.DeleteRole(batch.Data)),
                _ => StatusCode(405)
            };
        }

        /// <summary>
        /// 获取角色菜单
        /// </summary>
        /// <response code="200">成功</response>
        [HttpGet("{id}/menu")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MenuPermission(int id)
        {
            var (Menu, Permission) = await _basicQueryService.GetRoleMenuPermission(id);
            return StatusCode(200, new
            {
                Menu,
                Permission
            });
        }

        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <response code="200">成功</response>
        /// <response code="410">角色已删除</response>
        /// <response code="422">请求内容错误</response>
        [HttpPost("{id}/menu")]
        [ProducesResponseType(200)]
        [ProducesResponseType(410)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> MenuPermission(int id, IEnumerable<int> permission)
        {
            var result = await _basicCommandService.SetRoleMenuPermission(id, permission);
            return result switch
            {
                { Success: true } => StatusCode(200),
                { Failure: { ErrorCode: nameof(ErrorCode.Gone) } } => StatusCode(410),
                _ => StatusCode(422)
            };
        }
    }
}
