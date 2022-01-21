using Hangfire.Common;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Kean.Infrastructure.Hangfire
{
    /// <summary>
    /// 禁止重复执行任务
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DisallowConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter
    {
        private static readonly TimeSpan LOCK_TIMEOUT = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan FINGERPRINT_TIMEOUT = TimeSpan.FromMinutes(1);

        /*
         * 实现 Hangfire.Server.IServerFilter.OnPerforming(PerformingContext filterContext) 方法
         */
        public void OnPerforming(PerformingContext filterContext)
        {
            var job = $"{filterContext.BackgroundJob.Job.Type.FullName}-{filterContext.BackgroundJob.Job.Method.Name}-{string.Join('-', filterContext.BackgroundJob.Job.Args)}";
            using (filterContext.Connection.AcquireDistributedLock($"fingerprint:{job}:lock", LOCK_TIMEOUT))
            {
                var hash = filterContext.Connection.GetAllEntriesFromHash($"fingerprint:{job}");
                if (hash?.ContainsKey("Timestamp") == true &&
                    DateTime.TryParse(hash["Timestamp"], null, DateTimeStyles.RoundtripKind, out var timestamp) &&
                    DateTime.Now <= timestamp.Add(FINGERPRINT_TIMEOUT))
                {
                    filterContext.Canceled = true;
                }
                else
                {
                    filterContext.Connection.SetRangeInHash($"fingerprint:{job}", new Dictionary<string, string>
                    {
                        ["Timestamp"] = DateTime.Now.ToString("o")
                    });
                }
            }
        }

        /*
         * 实现 Hangfire.Server.IServerFilter.OnPerformed(PerformedContext filterContext) 方法
         */
        public void OnPerformed(PerformedContext filterContext)
        {
            var job = $"{filterContext.BackgroundJob.Job.Type.FullName}-{filterContext.BackgroundJob.Job.Method.Name}-{string.Join('-', filterContext.BackgroundJob.Job.Args)}";
            using (filterContext.Connection.AcquireDistributedLock($"fingerprint:{job}:lock", LOCK_TIMEOUT))
            {
                using (var transaction = filterContext.Connection.CreateWriteTransaction())
                {
                    transaction.RemoveHash($"fingerprint:{job}");
                    transaction.Commit();
                }
            }
        }
    }
}
