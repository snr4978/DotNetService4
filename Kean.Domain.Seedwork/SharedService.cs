using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 共享服务
    /// </summary>
    public sealed class SharedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _domain;

        /// <summary>
        /// 构造函数
        /// </summary>
        internal SharedService(IServiceProvider serviceProvider, string domain)
        {
            _serviceProvider = serviceProvider;
            _domain = domain;
        }

        /// <summary>
        /// 获取共享服务
        /// </summary>
        public Unit this[string index] =>
            new(_serviceProvider, _domain, index);

        /// <summary>
        /// 共享服务单元
        /// </summary>
        public sealed class Unit
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly string _domain;
            private readonly string _service;

            /// <summary>
            /// 构造函数
            /// </summary>
            internal Unit(IServiceProvider serviceProvider, string domain, string service)
            {
                _serviceProvider = serviceProvider;
                _domain = domain;
                _service = service;
            }

            /// <summary>
            /// 获取处理程序
            /// </summary>
            /// <param name="instance">服务实例</param>
            private MethodInfo GetHandler(out object instance)
            {
                var assembly = $"{GetType().Namespace}.{_domain}";
                var type = Type.GetType($"{assembly}.SharedServices.{_service},{assembly}");
                instance = _serviceProvider.GetService(type);
                return type.GetMethod("Handler");
            }

            /// <summary>
            /// 调用共享服务
            /// </summary>
            /// <param name="parameters">参数</param>
            public Task Invoke(params object[] parameters) =>
                GetHandler(out var instance).Invoke(instance, parameters) as Task;

            /// <summary>
            /// 调用共享服务
            /// </summary>
            /// <typeparam name="T">结果类型</typeparam>
            /// <param name="parameters">参数</param>
            /// <returns>操作结果</returns>
            public Task<T> Invoke<T>(params object[] parameters) =>
               GetHandler(out var instance).Invoke(instance, parameters) as Task<T>;
        }
    }
}
