using System;
using System.Security.Cryptography;
using System.Text;

namespace Kean.Infrastructure.Utilities
{
    /// <summary>
    /// Sha256加密
    /// </summary>
    public sealed class Sha256Encoding
    {
        /// <summary>
        /// 256位散列加密
        /// </summary>
        /// <param name="text">明文</param>
        /// <returns>密文</returns>
        public static string Encode(string text)
        {
            var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.Default.GetBytes(text));
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
    }
}
