using FluentValidation;

namespace Kean.Domain.Basic.Commands
{
    /// <summary>
    /// 修改角色命令
    /// </summary>
    public class ModifyRoleCommand : CommandValidator<ModifyRoleCommand>, ICommand
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int Id { get; set; }

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
            RuleFor(r => r.Id).NotEmpty().WithMessage("标识不允许空");
            RuleFor(r => r.Name).NotEmpty().WithMessage("名称不允许空");
        }
    }
}
