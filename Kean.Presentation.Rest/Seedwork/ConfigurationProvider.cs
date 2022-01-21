using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 配置文件的取值方式
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.ConfigurationProvider 类的新实例
        /// </summary>
        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
         * 实现 Microsoft.Extensions.Configuration.GetChildKeys 方法
         */
        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath) =>
            (parentPath == null ? _configuration : _configuration.GetSection(parentPath))
            .GetChildren()
            .Select(c => c.Key)
            .Concat(earlierKeys)
            .OrderBy(k => k, ConfigurationKeyComparer.Instance);

        /*
         * 实现 Microsoft.Extensions.Configuration.GetReloadToken 方法
         */
        public IChangeToken GetReloadToken() =>
            _configuration.GetReloadToken();

        /*
         * 实现 Microsoft.Extensions.Configuration.Set 方法
         */
        public void Set(string key, string value) =>
            _configuration[key] = value;

        /*
         * 实现 Microsoft.Extensions.Configuration.Load 方法
         */
        public void Load() { }

        /*
         * 实现 Microsoft.Extensions.Configuration.TryGet 方法
         */
        public bool TryGet(string key, out string value) =>
            (value = Interpolate(key)) != null;

        /*
         * 插值递归
         */
        private string Interpolate(string key)
        {
            var value = _configuration[key];
            if (value != null)
            {
                foreach (var item in new Regex(@"(?<=\$\{)[^\$\{\}]*(?=\})", RegexOptions.Compiled).Matches(value).Cast<Match>().SelectMany(m => m.Captures.Cast<Capture>()))
                {
                    value = value.Replace($"${{{item.Value}}}", Interpolate(item.Value));
                }
            }
            return value;
        }
    }
}
