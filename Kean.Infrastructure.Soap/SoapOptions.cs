namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// Soap 配置项
    /// </summary>
    public sealed class SoapOptions
    {
        /// <summary>
        /// 服务端集合
        /// </summary>
        public ServiceCollection Services { get; internal set; }
    }
}
