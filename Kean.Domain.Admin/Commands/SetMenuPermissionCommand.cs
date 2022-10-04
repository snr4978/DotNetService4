using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Admin.Commands
{
    /// <summary>
    /// 设置角色菜单权限命令
    /// </summary>
    public class SetMenuPermissionCommand : CommandValidator<SetMenuPermissionCommand>, ICommand
    {
        /// <summary>
        /// 角色 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public IEnumerable<int> Permission { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("角色 ID 不允许空");
        }
    }
}
