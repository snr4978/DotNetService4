namespace Kean.Infrastructure.SignalR
{
    /// <summary>
    /// SignalR 配置项
    /// </summary>
    public sealed class SignalROptions
    {
        /// <summary>
        /// Hub 集合
        /// </summary>
        public HubCollection Hubs { get; set; }
    }
}
