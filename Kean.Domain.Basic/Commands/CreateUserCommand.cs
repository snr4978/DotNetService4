using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Basic.Commands
{
    /// <summary>
    /// 创建用户命令
    /// </summary>
    public class CreateUserCommand : CommandValidator<CreateUserCommand>, ICommand
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<int> Role { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Name).NotEmpty().WithMessage("名称不允许空");
            RuleFor(r => r.Account).NotEmpty().WithMessage("账号不允许空");
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Output]
        public int Id { get; private set; }
    }
}
