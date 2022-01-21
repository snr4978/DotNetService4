using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    /// <summary>
    /// 事件处理程序
    /// </summary>
    /// <typeparam name="T">事件模型</typeparam>
    public abstract class EventHandler<T> : IEventHandler<T> where T : class, IEvent
    {
        /*
         * 抽象实现接口 Kean.Domain.IEventHandler<T>.Handle，表示事件处理
         * 实际上就是 MediatR.INotificationHandler<T>.Handle
         */
        public abstract Task Handle(T @event, CancellationToken cancellationToken);
    }
}
