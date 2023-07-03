using AutoMapper;
using Kean.Application.Query.Interfaces;
using Kean.Application.Query.ViewModels;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using Kean.Infrastructure.Utilities;
using System.Threading.Tasks;

namespace Kean.Application.Query.Implements
{
    /// <summary>
    /// 应用程序查询服务实现
    /// </summary>
    public class AppService : IAppService
    {
        private readonly IMapper _mapper; // 模型映射
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public AppService(
            IMapper mapper,
            IDefaultRedis redis)
        {
            _mapper = mapper;
            _redis = redis;
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAppService.GetBlacklist(string address) 方法
         */
        public async Task<Blacklist> GetBlacklist(string address)
        {
            var cache = await _redis.Hash["blacklist"].Get(address);
            var entity = cache == null ? null : JsonHelper.Deserialize<T_SYS_SECURITY>(cache);
            return _mapper.Map<Blacklist>(entity);
        }

        /*
         * 实现 Kean.Application.Query.Interfaces.IAppService.GetParam(string key) 方法
         */
        public async Task<string> GetParam(string key)
        {
            return await _redis.Hash["param"].Get(key);
        }
    }
}
