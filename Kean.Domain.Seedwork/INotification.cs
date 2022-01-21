using System;
using System.Collections.Generic;

namespace Kean.Domain
{
    /// <summary>
    /// 表示通知
    /// </summary>
    public interface INotification : IList<NotificationEvent>, IDisposable
    {
        // 该接口仅作为一个标识
    }
}
