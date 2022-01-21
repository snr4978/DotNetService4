namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// Hangfire 配置项
    /// </summary>
    public sealed class HangfireOptions
    {
        /// <summary>
        /// Redis 仓库
        /// </summary>
        public RedisStorage RedisStorage { get; set; } = new();

        /// <summary>
        /// 定时作业集合
        /// </summary>
        public RecurringJobCollection RecurringJobs { get; set; } = new();
    }
}
