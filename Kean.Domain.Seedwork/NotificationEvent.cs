namespace Kean.Domain
{
    /// <summary>
    /// 通知
    /// </summary>
    public sealed class NotificationEvent : IEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">消息码</param>
        /// <param name="errorMessage">消息内容</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="attemptedValue">属性值</param>
        public NotificationEvent(object errorCode, string errorMessage, string propertyName, object attemptedValue)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
            AttemptedValue = attemptedValue;
        }

        /// <summary>
        /// 获取消息码
        /// </summary>
        public object ErrorCode { get; }
        
        /// <summary>
        /// 获取消息内容
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// 获取属性名
        /// </summary>
        public string PropertyName { get; }
        
        /// <summary>
        /// 获取属性值
        /// </summary>
        public object AttemptedValue { get; }
    }
}
