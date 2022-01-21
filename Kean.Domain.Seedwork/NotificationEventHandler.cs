using System.Threading;
using System.Threading.Tasks;

namespace Kean.Domain
{
    public class NotificationEventHandler : IEventHandler<NotificationEvent>
    {
        protected INotification _notifications; // 通知

        /*
         * 构造函数
         */
        public NotificationEventHandler(INotification notifications) =>
            _notifications = notifications;

        /*
         * 事件处理程序
         */
        public virtual Task Handle(NotificationEvent @event, CancellationToken cancellationToken)
        {
            _notifications.Add(@event);
            return Task.CompletedTask;
        }
    }
}
