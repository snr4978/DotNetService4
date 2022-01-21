using Hangfire;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 抽象定时作业建造者
    /// </summary>
    public abstract class RecurringJobBuilder
    {
        /// <summary>
        /// 创建定时作业
        /// </summary>
        internal abstract void Build();
    }

    /// <summary>
    /// 定时作业建造者
    /// </summary>
    /// <typeparam name="T">定时作业类型</typeparam>
    public sealed class RecurringJobBuilder<T> : RecurringJobBuilder where T : IRecurringJob
    {
        private readonly string _cronExpression;

        /// <summary>
        /// 初始化 Kean.Infrastructure.Hangfire.RecurringJobBuilder 类的新实例
        /// </summary>
        /// <param name="cronExpression">cron 表达式</param>
        internal RecurringJobBuilder(string cronExpression)
        {
            _cronExpression = cronExpression;
        }

        /*
         * 实现 Kean.Infrastructure.Hangfire.RecurringJobBuilder.Build 方法
         */
        internal override void Build()
        {
            var recurringJobId = typeof(T).Name;
            RecurringJob.RemoveIfExists(recurringJobId);
            if (_cronExpression != null)
            {
                RecurringJob.AddOrUpdate<T>(recurringJobId, recurringJob => recurringJob.Execute(), _cronExpression);
            }
        }
    }
}
