using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Message.Commands
{
    /// <summary>
    /// 标记命令
    /// </summary>
    public class MarkMessageCommand : CommandValidator<MarkMessageCommand>, ICommand
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
        /// 状态标记
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.MessageId).NotEmpty().WithMessage("消息 ID 不合法");
        }
    }
}
