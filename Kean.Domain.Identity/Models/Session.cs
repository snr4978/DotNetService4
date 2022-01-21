using Kean.Infrastructure.Utilities;

namespace Kean.Domain.Identity.Models
{
    /// <summary>
    /// 会话
    /// </summary>
    public sealed class Session : ValueObject
    {
        /// <summary>
        /// 初始化 Kean.Domain.Identity.Models.Session 类的新实例
        /// </summary>
        /// <param name="token">令牌</param>
        public Session(string token)
        {
            Content = Sha256Encoding.Encode(token);
        }

        /// <summary>
        /// 会话内容
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 以字符串表示会话
        /// </summary>
        /// <param name="session">会话</param>
        public static implicit operator string(Session session) => session.Content;
    }
}
