using FluentValidation;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 身份验证命令
    /// </summary>
    public class AuthenticateCommand : CommandValidator<AuthenticateCommand>, ICommand
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Token).NotEmpty().WithMessage("令牌不允许空");
        }

        /// <summary>
        /// 身份
        /// </summary>
        [Output]
        public int? Identity { get; private set; }
    }
}
