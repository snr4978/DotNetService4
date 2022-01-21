namespace Kean.Infrastructure.Soap
{
    /// <summary>
    /// Soap 配置项
    /// </summary>
    public sealed class SoapOptions
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        public ClientCollection Clients { get; internal set; }

        /// <summary>
        /// 服务端集合
        /// </summary>
        public ServerCollection Servers { get; internal set; }
    }
}
