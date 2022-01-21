using System.Threading.Tasks;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 表示定时作业
    /// </summary>
    public interface IRecurringJob
    {
        /// <summary>
        /// 执行作业
        /// </summary>
        Task Execute();
    }
}
