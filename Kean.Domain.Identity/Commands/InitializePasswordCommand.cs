using FluentValidation;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 初始化密码命令
    /// </summary>
    public class InitializePasswordCommand : CommandValidator<InitializePasswordCommand>, ICommand
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Replacement).NotEmpty().WithMessage("密码不允许空");
        }
    }
}
