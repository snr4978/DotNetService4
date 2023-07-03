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
            return (int)(await GetRoleSchema(name)
                .Single(r => new { Count = Function.Count(r.ROLE_ID) }))
                .Count;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetRoleList(string name, string sort, int? offset, int? limit) 方法
         */
        public async Task<IEnumerable<Role>> GetRoleList(string name, string sort, int? offset, int? limit)
        {
            return _mapper.Map<IEnumerable<Role>>(await GetRoleSchema(name)
                .Sort<T_SYS_ROLE, Role>(sort, _mapper)
                .Page(offset, limit)
                .Select());
        }

        /*
         * 组织 GetRole 相关方法的条件
         */
        private ISchema<T_SYS_ROLE> GetRoleSchema(string name)
        {
            var schema = _database.From<T_SYS_ROLE>();
            if (name != null)
            {
                schema = schema.Where(r => r.ROLE_NAME.Contains(name));
            }
            return schema;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetRoleMenuPermission(int id) 方法
         */
        public async Task<(Tree<Menu> Menu, IEnumerable<int> Permission)> GetRoleMenuPermission(int id)
        {
            var menu = new Tree<Menu>(_mapper.Map<IEnumerable<Menu>>(await _database.From<T_SYS_MENU>()
                .OrderBy(m => m.MENU_ORDER, Infrastructure.Database.Order.Ascending)
                .OrderBy(m => m.MENU_ID, Infrastructure.Database.Order.Ascending)
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
            return (int)(await GetUserSchema(name, account, role)
                .Single(u => new { Count = Function.Count(u.USER_ID) }))
                .Count;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAdminService.GetUserList(string name, string account, int? role, string sort, int? offset, int? limit) 方法
         */
        public async Task<IEnumerable<User>> GetUserList(string name, string account, int? role, string sort, int? offset, int? limit)
        {
            var schema = GetUserSchema(name, account, role)
                .Sort<T_SYS_USER, User>(sort, _mapper)
                .Page(offset, limit);
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

        /*
         * 组织 GetUser 相关方法的条件
         */
        private ISchema<T_SYS_USER> GetUserSchema(string name, string account, int? role)
        {
            var schema = _database.From<T_SYS_USER>().Where(u => u.USER_ID > 0);
            if (role.HasValue)
            {
                var query = _database.From<T_SYS_USER_ROLE>().Where(r => r.ROLE_ID == role.Value).Query(r => r.USER_ID);
                schema = schema.Where(u => query.Contains(u.USER_ID));
            }
            if (name != null)
            {
                schema = schema.Where(u => u.USER_NAME.Contains(name));
            }
            if (account != null)
            {
                schema = schema.Where(u => u.USER_ACCOUNT.Contains(account));
            }
            return schema;
        }
    }
}
