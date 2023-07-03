using FluentValidation;

namespace Kean.Domain.App.Commands
{
    /// <summary>
    /// 配置系统参数命令
    /// </summary>
    public class ConfigParamCommand : CommandValidator<ConfigParamCommand>, ICommand
    {
        /// <summary>
        /// 参数键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 验证项
        /// </summary>
        protected override void Validation()
        {
            RuleFor(r => r.Key).NotEmpty().WithMessage("参数键不允许空");
        }
    }
}
