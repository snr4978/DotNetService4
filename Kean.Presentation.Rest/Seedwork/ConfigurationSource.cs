using Microsoft.Extensions.Configuration;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 配置文件来源
    /// </summary>
    public class ConfigurationSource : IConfigurationSource
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.ConfigurationSource 类的新实例
        /// </summary>
        public ConfigurationSource(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*
         * 实现 Microsoft.Extensions.Configuration.IConfigurationSource.Build 方法
         */
        public IConfigurationProvider Build(IConfigurationBuilder builder) => 
            new ConfigurationProvider(_configuration);
    }
}
