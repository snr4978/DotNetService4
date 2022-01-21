using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public sealed class CommandBus : ICommandBus
    {
        private readonly IMediator _mediator; // 中介者注入
        private readonly IUnitOfWork _unitOfWork; // 工作单元注入
        private readonly INotification _notification; // 通知注入

        /*
         * 构造函数
         * 命令总线基于 MediatR 实现
         */
        public CommandBus(IMediator mediator, IUnitOfWork unitOfWork, INotification notification)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        /*
         * 实现接口 Kean.Domain.ICommandBus.Execute ，表示执行命令
         * 基于 MediatR 请求方式
         */
        public async Task Execute(ICommand command, CancellationToken cancellationToken = default)
        {
            if (_unitOfWork.IsEntered)
            {
                // 一般在其他 Command 的 Handler 或相关 EventHandler 中执行命令，会到这里
                await _mediator.Send(command, cancellationToken);
            }
            else
            {
                // 一般首次执行 Command 会到这里，创建工作单元，同时开启事务
                using (_unitOfWork.Enter())
                {
                    await _mediator.Send(command, cancellationToken);
                    // 如果没有异常通知则提交事务，否则回滚（Dispose时会回滚）
                    if (_notification.Count == 0)
                    {
                        _unitOfWork.Exit();
                    }
                }
            }
        }

        /*
         * 实现接口 Kean.Domain.ICommandBus.Trigger ，表示触发事件
         * 基于 MediatR 广播方式
         */
        public async Task Trigger(IEvent @event, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        /*
         * 实现接口 Kean.Domain.ICommandBus.Notify ，表示发送通知
         * 主要用于 Command 和 Event 的异常信息缓存
         * 基于 MediatR 广播方式
         */
        public async Task Notify(string errorMessage, string propertyName = default, object attemptedValue = default, object errorCode = default, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(new NotificationEvent(errorCode, propertyName, errorMessage, attemptedValue), cancellationToken);
        }

        /*
         * 实现接口 Kean.Domain.ICommandBus.Notify ，表示发送通知
         * 针对 Command 中 ValidationResult 的缓存
         * 基于 MediatR 广播方式
         */
        public async Task Notify(ValidationResult validationResult, CancellationToken cancellationToken = default)
        {
            foreach (var item in validationResult.Errors)
            {
                await _mediator.Publish(new NotificationEvent(item.ErrorCode, item.ErrorMessage, item.PropertyName, item.AttemptedValue), cancellationToken);
            }
        }
    }
}
