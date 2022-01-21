namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 注销命令
    /// </summary>
    public class LogoutCommand : ICommand
    {
        /// <summary>
        /// 会话
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
}
