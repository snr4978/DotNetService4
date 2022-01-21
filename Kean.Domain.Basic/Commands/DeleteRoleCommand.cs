using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Basic.Commands
{
    /// <summary>
    /// 删除角色命令
    /// </summary>
    public class DeleteRoleCommand : CommandValidator<DeleteRoleCommand>, ICommand
    {
        /// <summary>
        /// 标识
        /// </summary>
        public IEnumerable<int> Id { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("标识不允许空");
        }
    }
}
