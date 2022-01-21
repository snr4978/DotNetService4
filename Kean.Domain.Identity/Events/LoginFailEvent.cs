namespace Kean.Domain.Identity.Events
{
    /// <summary>
    /// 登录失败时触发的事件
    /// </summary>
    public class LoginFailEvent : IEvent
    {
        /// <summary>
        /// 远程 Ip
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// UA 信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}
