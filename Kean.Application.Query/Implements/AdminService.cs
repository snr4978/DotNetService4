using AutoMapper;
using Kean.Application.Query.Interfaces;
using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Database;
using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Query.Implements
{
    /// <summary>
    /// 基础信息查询服务实现
    /// </summary>
    public sealed class AdminService : IAdminService
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultDb _database; // 默认数据库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AdminService(
            IMapper mapper,
            IDefaultDb database)
        {
            _mapper = mapper;
            _database = database;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetRoleCount(string name) 方法
         */
        public async Task<int> GetRoleCount(string name)
        {
            var schema = _database.From<T_SYS_ROLE>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                schema = schema.Where(r => r.ROLE_NAME.Contains(name));
            }
            return (await schema.Single(r => new { Count = Function.Count(r.ROLE_ID) })).Count;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetRoleList(string name, string sort, int? offset, int? limit) 方法
         */
        public async Task<IEnumerable<Role>> GetRoleList(string name, string sort, int? offset, int? limit)
        {
            var schema = _database.From<T_SYS_ROLE>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                schema = schema.Where(r => r.ROLE_NAME.Contains(name));
            }
            if (!string.IsNullOrEmpty(sort))
            {
                var order = sort[0] == '~' ? Order.Descending : Order.Ascending;
                var column = order == Order.Descending ? sort[1..] : sort;
                var expression = _mapper.GetPropertyMapExpression<T_SYS_ROLE, Role>(column);
                if (expression != null)
                {
                    schema = schema.OrderBy(expression, order);
                }
            }
            if (offset.HasValue)
            {
                schema = schema.Skip(offset.Value);
            }
            if (limit.HasValue)
            {
                schema = schema.Take(limit.Value);
            }
            return _mapper.Map<IEnumerable<Role>>(await schema.Select());
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetRoleMenuPermission(int id) 方法
         */
        public async Task<(Tree<Menu> Menu, IEnumerable<int> Permission)> GetRoleMenuPermission(int id)
        {
            var menu = new Tree<Menu>(_mapper.Map<IEnumerable<Menu>>(await _database.From<T_SYS_MENU>()
                .OrderBy(m => m.MENU_ORDER, Order.Ascending)
                .Where(m => m.MENU_FLAG == true)
                .Select()), "Id", "Parent");
            var permission = (await _database.From<T_SYS_ROLE_MENU>()
                .Where(r => r.ROLE_ID == id)
                .Select(r => new { r.MENU_ID }))
                .Select(r =>
                {
                    int menuId = r.MENU_ID;
                    return menuId;
                });
            return (menu, permission);
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetUserCount(string name, string account, int? role) 方法
         */
        public async Task<int> GetUserCount(string name, string account, int? role)
        {
            var schema = _database.From<T_SYS_USER>().Where(u => u.USER_ID > 0);
            if (role.HasValue)
            {
                var query = _database.From<T_SYS_USER_ROLE>().Where(r => r.ROLE_ID == role.Value).Query(r => r.USER_ID);
                schema = schema.Where(u => query.Contains(u.USER_ID));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                schema = schema.Where(u => u.USER_NAME.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(account))
            {
                schema = schema.Where(u => u.USER_ACCOUNT.Contains(account));
            }
            return (await schema.Single(u => new { Count = Function.Count(u.USER_ID) })).Count;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetUserList(string name, string account, int? role, string sort, int? offset, int? limit) 方法
         */
        public async Task<IEnumerable<User>> GetUserList(string name, string account, int? role, string sort, int? offset, int? limit)
        {
            var schema = _database.From<T_SYS_USER>().Where(u => u.USER_ID > 0);
            if (role.HasValue)
            {
                var query = _database.From<T_SYS_USER_ROLE>().Where(r => r.ROLE_ID == role.Value).Query(r => r.USER_ID);
                schema = schema.Where(u => query.Contains(u.USER_ID));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                schema = schema.Where(u => u.USER_NAME.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(account))
            {
                schema = schema.Where(u => u.USER_ACCOUNT.Contains(account));
            }
            if (!string.IsNullOrEmpty(sort))
            {
                var order = sort[0] == '~' ? Order.Descending : Order.Ascending;
                var column = order == Order.Descending ? sort[1..] : sort;
                var expression = _mapper.GetPropertyMapExpression<T_SYS_USER, User>(column);
                if (expression != null)
                {
                    schema = schema.OrderBy(expression, order);
                }
            }
            if (offset.HasValue)
            {
                schema = schema.Skip(offset.Value);
            }
            if (limit.HasValue)
            {
                schema = schema.Take(limit.Value);
            }
            return (await schema.Select())
                .Select(async u =>
                {
                    var user = _mapper.Map<User>(u);
                    var roles = _database.From<T_SYS_USER_ROLE>().Where(r => r.USER_ID == user.Id).Query(r => r.ROLE_ID);
                    user.Role = _mapper.Map<IEnumerable<Role>>(await _database.From<T_SYS_ROLE>().Where(r => roles.Contains(r.ROLE_ID)).Select());
                    return user;
                })
                .Select(t => t.Result);
        }
    }
}
