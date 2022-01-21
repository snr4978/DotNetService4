using Kean.Infrastructure.Hangfire;
using System;
using System.Threading.Tasks;

namespace Kean.Presentation.Rest.Jobs
{
    public class MyJob : IRecurringJob
    {
        [DisallowConcurrentExecution]
        public Task Execute()
        {
            //Console.WriteLine(DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
