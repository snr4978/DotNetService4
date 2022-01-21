using FluentValidation;

namespace Kean.Domain.Message.Commands
{
    /// <summary>
    /// 连线命令
    /// </summary>
    public class ConnectCommand : CommandValidator<ConnectCommand>, ICommand
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 连接码
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Token).NotEmpty().WithMessage("令牌不允许空");
            RuleFor(r => r.Id).NotEmpty().WithMessage("连接码不允许空");
        }
    }
}
