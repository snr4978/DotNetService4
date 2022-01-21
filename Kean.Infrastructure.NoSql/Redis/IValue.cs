namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// 表示数据类型
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    public interface IValue<T>
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        T this[string key] { get; }
    }
}
