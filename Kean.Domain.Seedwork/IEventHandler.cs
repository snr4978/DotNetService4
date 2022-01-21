using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 表示事件处理程序
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    internal interface IEventHandler<in T> : INotificationHandler<T> where T : class, IEvent
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="event">事件</param>
        /// <param name="cancellationToken">取消标记</param>
        new Task Handle(T @event, CancellationToken cancellationToken);
    }
}
