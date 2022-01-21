namespace Kean.Domain
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown = 400,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,
        
        /// <summary>
        /// 禁止
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 禁用操作
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// 冲突
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// 丢失
        /// </summary>
        Gone = 410,

        /// <summary>
        /// 失效
        /// </summary>
        Expired = 419,

        /// <summary>
        /// 请求内容错误
        /// </summary>
        Unprocessable = 422,

        /// <summary>
        /// 锁定
        /// </summary>
        Locked = 423,

        /// <summary>
        /// 前置条件不满足
        /// </summary>
        Precondition = 428
    }
}
