using FluentValidation;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 修改密码命令
    /// </summary>
    public class ModifyPasswordCommand : CommandValidator<ModifyPasswordCommand>, ICommand
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 当前密码
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Current).NotEmpty().WithMessage("当前密码不允许空");
            RuleFor(r => r.Replacement).NotEmpty().WithMessage("新密码不允许空");
        }
    }
}
