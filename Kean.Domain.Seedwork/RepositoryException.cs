using System;
using System.Collections.Generic;

namespace Kean.Domain
{
    /// <summary>
    /// 仓库异常
    /// </summary>
    public sealed class RepositoryException : Exception
    {
        /// <summary>
        /// 初始化 Kean.Domain.RepositoryException 类的新实例
        /// </summary>
        /// <param name="message">异常信息</param>
        /// <param name="member">成员信息</param>
        public RepositoryException(string message, KeyValuePair<string, object> member)
            : base(message) => Member = member;

        /// <summary>
        /// 成员信息
        /// </summary>
        public KeyValuePair<string, object> Member { get; set; }
    }
}
