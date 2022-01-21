using System;

namespace Kean.Domain
{
    /// <summary>
    /// 包含共享的域索引
    /// </summary>
    public sealed class Domain : IDomain
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public Domain(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider;

        /*
         * 实现索引
         */
        public (string Name, SharedService SharedService) this[string index] =>
            (index, new(_serviceProvider, index));
    }
}
