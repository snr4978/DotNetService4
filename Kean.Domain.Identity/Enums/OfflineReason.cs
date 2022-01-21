namespace Kean.Domain.Identity.Enums
{
    /// <summary>
    /// 离线原因
    /// </summary>
    public enum OfflineReason
    {
        /// <summary>
        /// 过期
        /// </summary>
        Timeout,

        /// <summary>
        /// 冲突
        /// </summary>
        Conflict,

        /// <summary>
        /// 重启服务
        /// </summary>
        Restart
    }
}
