using Kean.Infrastructure.Database.Repository.Default;
using Kean.Infrastructure.Database.Repository.Default.Entities;
using Kean.Infrastructure.NoSql.Repository.Default;
using System.Linq;
using System.Threading.Tasks;

namespace Kean.Infrastructure.Repository
{
    /// <summary>
    /// 参数仓库
    /// </summary>
    public class ParamRepository :
        Domain.App.Repositories.IParamRepository
    {
        private readonly IDefaultDb _database; // 默认数据库
        private readonly IDefaultRedis _redis; // 默认 Redis

        /// <summary>
        /// 依赖注入
        /// </summary>
        public ParamRepository(
            IDefaultDb database,
            IDefaultRedis redis)
        {
            _database = database;
            _redis = redis;
        }

        /*
         * 实现 Kean.Domain.App.Repositories.IParamRepository.LoadParam() 方法
         */
        public async Task LoadParam()
        {
            var @params = await _database.From<T_SYS_PARAM>().Select();
            await _redis.Batch(batch =>
            {
                var hash = batch.Hash["param"];
                return batch.Execute(@params.Select(p => hash.Set(p.PARAM_KEY, p.PARAM_VALUE)).ToArray());
            });
        }
    }
}
