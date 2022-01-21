using System.Collections.Generic;

namespace Kean.Domain
{
    /// <summary>
    /// 通知
    /// </summary>
    public sealed class Notification : List<NotificationEvent>, INotification
    {
        /*
         * 实现 System.IDisposable.Dispose
         * 清空了集合，实际上没啥用
         */
        public void Dispose() => 
            Clear();
    }
}
