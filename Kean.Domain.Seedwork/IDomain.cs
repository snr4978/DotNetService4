namespace Kean.Domain
{
    /// <summary>
    /// 表示 Domain 信息
    /// </summary>
    public interface IDomain
    {
        /// <summary>
        /// 获取 Domain 信息
        /// </summary>
        /// <param name="index">标识</param>
        /// <returns>Domain 信息</returns>
        (string Name, SharedService SharedService) this[string index] { get; }
    }
}
