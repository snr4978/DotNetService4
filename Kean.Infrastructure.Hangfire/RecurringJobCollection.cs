using System.Collections.Generic;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 定时作业集合
    /// </summary>
    public sealed class RecurringJobCollection
    {
        private readonly IList<RecurringJobBuilder> _list = new List<RecurringJobBuilder>();

        /// <summary>
        /// 添加定时作业
        /// </summary>
        /// <typeparam name="T">定时作业类型</typeparam>
        /// <param name="cronExpression">cron 表达式</param>
        public void Add<T>(string cronExpression) where T : IRecurringJob =>
            _list.Add(new RecurringJobBuilder<T>(cronExpression));

        /// <summary>
        /// 返回一个循环访问定时作业建造者集合的枚举器
        /// </summary>
        /// <returns>用于循环访问集合的枚举数</returns>
        public IEnumerator<RecurringJobBuilder> GetEnumerator() =>
            _list.GetEnumerator();
    }
}
