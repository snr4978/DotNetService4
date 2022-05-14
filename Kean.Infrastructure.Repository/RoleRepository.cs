using AutoMapper;
using Kean.Domain.Basic.Models;
using Kean.Domain.Basic.Repositories;
using Kean.Infrastructure.Database;
using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 角色仓库
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultDb _database; // 默认数据库

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleRepository(
            IMapper mapper,
            IDefaultDb database)
        {
            _mapper = mapper;
            _database = database;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.IsExist(int id) 方法
         */
        public async Task<bool> IsExist(int id)
        {
            return (await _database.From<T_SYS_ROLE>()
                .Where(r => r.ROLE_ID == id)
                .Single(r => new { Count = Function.Count(r.ROLE_ID) })).Count > 0;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.IsExist(string name, int? igrone) 方法
         */
        public async Task<bool> IsExist(string name, int? igrone)
        {
            var schema = _database.From<T_SYS_ROLE>().Where(r => r.ROLE_NAME == name);
            if (igrone.HasValue)
            {
                schema = schema.Where(r => r.ROLE_ID != igrone.Value);
            }
            return (await schema.Single(r => new { Count = Function.Count(r.ROLE_ID) })).Count > 0;
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.Create(Role role) 方法
         */
        public async Task<int> Create(Role role)
        {
            var id = await _database.From<T_SYS_ROLE>()
                .Add(_mapper.Map<T_SYS_ROLE>(role));
            return Convert.ToInt32(id);
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.Modify(Role role) 方法
         */
        public Task Modify(Role role)
        {
            return _database.From<T_SYS_ROLE>()
                .Update(_mapper.Map<T_SYS_ROLE>(role));
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.Delete(int id) 方法
         */
        public async Task Delete(int id)
        {
            await _database.From<T_SYS_ROLE>()
                .Where(r => r.ROLE_ID == id)
                .Delete();
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.SetMenuPermission(int id, IEnumerable<int> permission) 方法
         */
        public async Task SetMenuPermission(int id, IEnumerable<int> permission)
        {
            await _database.From<T_SYS_ROLE_MENU>()
                .Where(r => r.ROLE_ID == id)
                .Delete();
            foreach (var item in permission)
            {
                await _database.From<T_SYS_ROLE_MENU>().Add(new()
                {
                    ROLE_ID = id,
                    MENU_ID = item
                });
            }
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.ClearMenuPermission(int id) 方法
         */
        public async Task ClearMenuPermission(int id)
        {
            await _database.From<T_SYS_ROLE_MENU>()
                .Where(r => r.ROLE_ID == id)
                .Delete();
        }

        /*
         * 实现 Kean.Domain.Basic.Repositories.IRoleRepository.ClearUserRole(int id) 方法
         */
        public async Task ClearUserRole(int id)
        {
            await _database.From<T_SYS_USER_ROLE>()
                .Where(r => r.ROLE_ID == id)
                .Delete();
        }
    }
}
