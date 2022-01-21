using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kean.Infrastructure.NoSql.Redis
{
    /// <summary>
    /// Redis 配置管理
    /// </summary>
    public static class Configuration
    {
        private static readonly ConcurrentDictionary<string, IDriver> drivers = new();

        /// <summary>
        /// 配置 Redis 驱动
        /// </summary>
        /// <param name="config">AppSetting 中的配置节</param>
        public static IDriver Configure(string config)
        {
            config ??= string.Empty;
            if (!drivers.ContainsKey(config))
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                Dictionary<string, string> param;
                if (config == string.Empty)
                {
                    param = configuration
                        .GetSection("NoSql:Redis")
                        .GetChildren()
                        .First()
                        .GetChildren()
                        .ToDictionary(i => i.Key, i => i.Value);
                }
                else
                {
                    param = configuration
                        .GetSection($"NoSql:Redis:{config}")
                        .GetChildren()
                        .ToDictionary(i => i.Key, i => i.Value);
                }
                drivers.TryAdd(config, Activator.CreateInstance(Type.GetType(param["DriverClass"]), param["ConnectionString"], int.TryParse(param["Database"], out var i) ? i : 0) as IDriver);
            }
            return drivers[config];
        }
    }
}
