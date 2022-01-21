using FluentValidation;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class LoginCommand : CommandValidator<LoginCommand>, ICommand
    {
        /// <summary>
        /// 远程 Ip
        /// </summary>
        public string RemoteIp { get; set; }

        /// <summary>
        /// 设置 UA 信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Account).NotEmpty().WithMessage("账号不允许空");
            RuleFor(r => r.Password).NotEmpty().WithMessage("密码不允许空");
        }

        /// <summary>
        /// 令牌
        /// </summary>
        [Output]
        public string Token { get; private set; }
    }
}
