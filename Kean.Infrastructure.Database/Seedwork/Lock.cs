namespace Kean.Infrastructure.Database
{
    /// <summary>
    /// 锁
    /// </summary>
    public enum Lock
    {
        /// <summary>
        /// 保持锁
        /// </summary>
        Holdlock,

        /// <summary>
        /// 无锁
        /// </summary>
        Nolock,

        /// <summary>
        /// 页锁
        /// </summary>
        Paglock,

        /// <summary>
        /// 行锁
        /// </summary>
        Rowlock,

        /// <summary>
        /// 表锁
        /// </summary>
        Tablock,

        /// <summary>
        /// 排他表锁
        /// </summary>
        Tablockx,

        /// <summary>
        /// 更新锁
        /// </summary>
        Updlock,

        /// <summary>
        /// 排他锁
        /// </summary>
        Xlock
    }
}
