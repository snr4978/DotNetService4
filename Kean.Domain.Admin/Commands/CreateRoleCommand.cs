using FluentValidation;

namespace Kean.Domain.Admin.Commands
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class CreateRoleCommand : CommandValidator<CreateRoleCommand>, ICommand
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("名称不允许空");
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Output]
        public int Id { get; private set; }
    }
}
