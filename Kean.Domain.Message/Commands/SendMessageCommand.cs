using FluentValidation;
using System.Collections.Generic;

namespace Kean.Domain.Message.Commands
{
    /// <summary>
    /// 发送命令
    /// </summary>
    public class SendMessageCommand : CommandValidator<SendMessageCommand>, ICommand
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息源（发送者的用户 ID）
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 目标（接收者的用户 ID 集合）
        /// </summary>
        public IEnumerable<int> Targets { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Subject).NotEmpty().WithMessage("主题不允许为空");
            RuleFor(r => r.Targets).NotEmpty().WithMessage("目标不允许为空");
        }
    }
}
