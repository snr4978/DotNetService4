using Kean.Domain.App.Commands;
using Kean.Domain.App.Repositories;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain.App.CommandHandlers
{
    /// <summary>
    /// 配置系统参数命令处理程序
    /// </summary>
    public sealed class ConfigParamCommandHandler : CommandHandler<ConfigParamCommand>
    {
        private readonly ICommandBus _commandBus; // 命令总线
        private readonly IParamRepository _paramRepository; // 参数仓库

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigParamCommandHandler(
            ICommandBus commandBus,
            IParamRepository paramRepository)
        {
            _commandBus = commandBus;
            _paramRepository = paramRepository;
        }

        /// <summary>
        /// 处理程序
        /// </summary>
        public override async Task Handle(ConfigParamCommand command, CancellationToken cancellationToken)
        {
            if (command.ValidationResult.IsValid)
            {
                var validation = await _paramRepository.GetValidation(command.Key);
                if (validation == null)
                {
                    await _commandBus.Notify(nameof(command.Key), "参数不允许操作", command.Key,
                        cancellationToken: cancellationToken);
                    return;
                }
                else if (validation != string.Empty)
                {
                    if (!new Regex(validation).IsMatch(command.Value))
                    {
                        await _commandBus.Notify(nameof(command.Value), "参数内容不正确", command.Value,
                            cancellationToken: cancellationToken);
                        return;
                    }
                }
                await _paramRepository.SetValue(command.Key, command.Value);
            }
            else
            {
                await _commandBus.Notify(command.ValidationResult,
                    cancellationToken: cancellationToken);
            }
        }
    }
}
