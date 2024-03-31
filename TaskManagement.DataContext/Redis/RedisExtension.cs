using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TaskManagement.DataContext
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedisDbContext(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("RedisConnectionString");

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Connection string can't be null or empty.");

            ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(connectionString);

            services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(configurationOptions));
            services.AddScoped<RedisDbContext>();

            return services;
        }
    }
}
