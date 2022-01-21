using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 表示命令总线
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令模型</param>
        /// <param name="cancellationToken">取消标记</param>
        Task Execute(ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="event">事件模型</param>
        /// <param name="cancellationToken">取消标记</param>
        Task Trigger(IEvent @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="errorMessage">消息内容</param>
        /// <param name="attemptedValue">属性值</param>
        /// <param name="errorCode">消息码</param>
        /// <param name="cancellationToken">取消标记</param>
        Task Notify(string propertyName, string errorMessage = default, object attemptedValue = default, object errorCode = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发送通知
        /// </summary>
        /// <param name="validationResult">验证结果</param>
        /// <param name="cancellationToken">取消标记</param>
        Task Notify(ValidationResult validationResult, CancellationToken cancellationToken = default);
    }
}
