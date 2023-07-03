using Kean.Infrastructure.Hangfire;
using Kean.Infrastructure.SignalR;
using Kean.Infrastructure.Soap;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kean.Presentation.Rest
{
    /// <summary>
    /// 应用程序启动类
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 初始化 Kean.Presentation.Rest.Startup 类的新实例
        /// </summary>
        public Startup(IConfiguration configuration) =>
            _configuration = configuration;

        /// <summary>
        /// 依赖注入
        /// </summary>
        private static IServiceCollection DependencyInject(IServiceCollection services) => services
            .Add<Infrastructure.Database.Repository.DependencyInjection>()
            .Add<Infrastructure.NoSql.Repository.DependencyInjection>()
            .Add<Infrastructure.Repository.DependencyInjection>()
            .Add<Domain.Seedwork.DependencyInjection>()
            .Add<Domain.Admin.DependencyInjection>()
            .Add<Domain.App.DependencyInjection>()
            .Add<Domain.Identity.DependencyInjection>()
            .Add<Domain.Message.DependencyInjection>()
            .Add<Application.Command.DependencyInjection>()
            .Add<Application.Query.DependencyInjection>();

        /// <summary>
        /// 将服务添加到容器
        /// </summary>
        public void ConfigureServices(IServiceCollection services) =>
            DependencyInject(services
                .AddSwaggerGen(options =>
                {
                    options.OperationFilter<SwaggerFilter>();
                    options.CustomSchemaIds(_ => Guid.NewGuid().ToString("N"));
                    options.OrderActionsBy(a => a.RelativePath);
                    //options.IncludeXmlComments("Swagger.xml", true);
                    options.SwaggerDoc(_configuration["Swagger:Version"], new()
                    {
                        Title = _configuration["Swagger:Title"],
                        Version = _configuration["Swagger:Version"],
                        Contact = new()
                        {
                            Name = _configuration["Swagger:Contact:Name"],
                            Email = _configuration["Swagger:Contact:Email"]
                        }
                    });
                })
                //.AddHangfire(options =>
                //{
                //    options.RedisStorage.ConnectionString = _configuration["Hangfire:RedisStorage:ConnectionString"];
                //    options.RedisStorage.Database = int.Parse(_configuration["Hangfire:RedisStorage:Database"]);
                //    options.RecurringJobs.Add<Jobs.MyJob>(_configuration["Hangfire:RecurringJobs:MyJob"]);
                //})
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins(_configuration["AllowedOrigins"].Split(';'))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                })
                .AddSignalR(options =>
                {
                    options.Hubs.Add<Domain.Identity.Sockets.IOnlineSocket, Hubs.IdentityHub>();
                    options.Hubs.Add<Domain.Message.Sockets.IOnlineSocket, Hubs.MessageHub>();
                })
                //.AddSoap(options =>
                //{
                //    options.Servers.Add<Soaps.Contracts.MyServiceContract, Soaps.Services.MyService>();
                //})
                .AddControllers(options =>
                {
                    options.Filters.Add<ActionFilter>();
                    options.Filters.Add<ExceptionFilter>();
                    options.InputFormatters.Add(new TextPlainInputFormatter());
                }))
            .Startup();

        /// <summary>
        /// 配置 HTTP 请求管道
        /// </summary>
        public void Configure(IApplicationBuilder app) => app
            //.UseHangfire()
            .UseSwagger()
            .UseRouting()
            .UseCors()
            .UseBlacklist()
            .UseAuthentication()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHubs();
                //endpoints.MapSoaps();
            });
    }
}
