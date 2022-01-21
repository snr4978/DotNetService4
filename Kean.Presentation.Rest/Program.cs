using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using Serilog;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 应用程序入口
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口
        /// </summary>
        public static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, builder) => builder.Add(new ConfigurationSource(builder.Build())))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));
                    webBuilder.UseStartup<Startup>();
                });
    }
}
