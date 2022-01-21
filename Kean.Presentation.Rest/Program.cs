using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using Serilog;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// Ӧ�ó������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ���
        /// </summary>
        public static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// ��ʼ��
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
