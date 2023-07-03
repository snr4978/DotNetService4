using AutoMapper;
using Kean.Application.Query.Interfaces;
using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Database;
using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using Kean.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Application.Query.Implements
{
    /// <summary>
    /// 身份查询服务实现
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultDb _database; // 默认数据库
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public IdentityService(
            IMapper mapper,
            IDefaultDb database,
            IDefaultRedis redis)
        {
            _mapper = mapper;
            _database = database;
            _redis = redis;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IIdentityService.GetUser(int id) 方法
         */
        public async Task<User> GetUser(int id)
        {
            if (await _redis.Hash[$"identity:{id}"].Get("tag") == "super")
            {
                var super = await _redis.Hash["param"].Get("super_user");
                if (super != null)
                {
                    return _mapper.Map<User>(JsonHelper.Deserialize<T_SYS_USER>(super));
                }
            }
            return _mapper.Map<User>(await _database.From<T_SYS_USER>()
                .Where(u => u.USER_ID == id)
                .Single());
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IIdentityService.GetMenu(int id) 方法
         */
        public async Task<IEnumerable<Menu>> GetMenu(int id)
        {
            var menu = await _database.From<T_SYS_MENU>()
                .OrderBy(m => m.MENU_ORDER, Infrastructure.Database.Order.Ascending)
                .OrderBy(m => m.MENU_ID, Infrastructure.Database.Order.Ascending)
                .Where(m => m.MENU_TYPE == "Menu" && m.MENU_FLAG == true)
                .Select();
            if (await _redis.Hash[$"identity:{id}"].Get("tag") == "super")
            {
                return _mapper.Map<IEnumerable<Menu>>(menu);
            }
            var permission = await _database.From<T_SYS_ROLE_MENU, T_SYS_USER_ROLE>()
                .Join(Join.Inner, (rm, ur) => rm.ROLE_ID == ur.ROLE_ID && ur.USER_ID == id)
                .Distinct()
                .Select((rm, _) => new { rm.MENU_ID });
            return menu.Join(permission,
                m => m.MENU_ID,
                p => p.MENU_ID,
                (m, _) => _mapper.Map<Menu>(m));
        }
    }
}
