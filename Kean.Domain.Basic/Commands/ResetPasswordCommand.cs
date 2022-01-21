using FluentValidation;

namespace Kean.Domain.Basic.Commands
{
    /// <summary>
    /// 重置密码命令
    /// </summary>
    public class ResetPasswordCommand : CommandValidator<ResetPasswordCommand>, ICommand
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("标识不允许空");
        }
    }
}
