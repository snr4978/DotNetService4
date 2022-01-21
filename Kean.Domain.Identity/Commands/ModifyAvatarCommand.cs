using FluentValidation;

namespace Kean.Domain.Identity.Commands
{
    /// <summary>
    /// 修改头像命令
    /// </summary>
    public class ModifyAvatarCommand : CommandValidator<ModifyAvatarCommand>, ICommand
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Avatar.Length).GreaterThan(8000).WithMessage("内容过大");
        }
    }
}
