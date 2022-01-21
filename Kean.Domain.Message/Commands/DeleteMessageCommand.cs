using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Message.Commands
{
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteMessageCommand : CommandValidator<DeleteMessageCommand>, ICommand
    {
        /// <summary>
        /// 用户 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 消息 ID
        /// </summary>
        public IEnumerable<int> MessageId { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.UserId).NotEmpty().WithMessage("用户 ID 不合法");
            RuleFor(r => r.MessageId).NotEmpty().WithMessage("消息 ID 不合法");
        }
    }
}
