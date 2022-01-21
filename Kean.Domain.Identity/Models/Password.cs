using Kean.Infrastructure.Utilities;

namespace Kean.Domain.Identity.Models
{
    /// <summary>
    /// 密码
    /// </summary>
    public sealed class Password : ValueObject
    {
        /// <summary>
        /// 初始化 Kean.Domain.Identity.Models.Password 类的新实例
        /// </summary>
        /// <param name="clearText">明文</param>
        public Password(string clearText)
        {
            CipherText = Sha256Encoding.Encode(ClearText = clearText);
        }

        /// <summary>
        /// 明文
        /// </summary>
        public string ClearText { get; }

        /// <summary>
        /// 密文
        /// </summary>
        public string CipherText { get; }

        /// <summary>
        /// 以字符串表示密文
        /// </summary>
        /// <param name="password">密码</param>
        public static implicit operator string(Password password) => password.CipherText;
    }
}
