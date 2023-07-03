using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 导航命令
    /// </summary>
    public class NavigateCommand : CommandValidator<NavigateCommand>, ICommand
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 忽略项
        /// </summary>
        public string[] Ignore { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Token).NotEmpty().WithMessage("令牌不允许空");
            RuleFor(r => r.Url).NotEmpty().WithMessage("URL 不允许空");
        }

        /// <summary>
        /// 权限信息
        /// </summary>
        [Output]
        public IEnumerable<string> Permission { get; private set; }
    }
}
