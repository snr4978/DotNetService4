using Kean.Domain.App.Repositories;
using Kean.Domain.Shared;
using System.Threading.Tasks;

namespace Kean.Domain.App
{
    /// <summary>
    /// 共享服务
    /// </summary>
    public class SharedService : IAppService
    {
        private readonly IParamRepository _paramRepository; // 参数仓库

        /// <summary>
        /// 依赖注入
        /// </summary>
        public SharedService(
            IParamRepository paramRepository)
        {
            _paramRepository = paramRepository;
        }

        /*
         * 实现 Kean.Domain.Shared.IAppService.GetParam(string key) 方法
         */
        public Task<string> GetParam(string key) =>
            _paramRepository.GetValue(key);
    }
}
