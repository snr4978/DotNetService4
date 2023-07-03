using Kean.Domain;

namespace Kean.Application.Command.ViewModels
{
    /// <summary>
    /// 失败信息
    /// </summary>
    public sealed class Failure
    {
        /// <summary>
        /// 根据 FluentValidation.Results.ValidationFailure 创建实例
        /// </summary>
        public static implicit operator Failure(NotificationEvent @event) =>
            @event == null ?
            null :
            new()
            {
                ErrorCode = @event.ErrorCode,
                ErrorMessage = @event.ErrorMessage,
                PropertyName = @event.PropertyName,
                AttemptedValue = @event.AttemptedValue
            };

        /// <summary>
        /// 消息码
        /// </summary>
        public object ErrorCode { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public object AttemptedValue { get; set; }

        /*
         * 重写 ToString 方法
         */
        public override string ToString() =>
            $"{ErrorMessage}（{PropertyName}：{AttemptedValue}）";
    }
}
