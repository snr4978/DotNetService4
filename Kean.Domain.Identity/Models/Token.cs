using System;

namespace Kean.Domain.Identity.Models
{
    /// <summary>
    /// 令牌
    /// </summary>
    public sealed class Token : ValueObject
    {
        /// <summary>
        /// 初始化 Kean.Domain.Identity.Models.Token 类的新实例
        /// </summary>
        public Token()
        {
            Content = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 令牌内容
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 以字符串表示令牌
        /// </summary>
        /// <param name="token">令牌</param>
        public static implicit operator string(Token token) => token.Content;
    }
}
