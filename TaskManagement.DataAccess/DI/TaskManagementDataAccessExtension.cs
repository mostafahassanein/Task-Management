using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.DataAccess.Implementations;
using TaskManagement.DataAccess.Interfaces;
using TaskManagement.DataContext;

namespace TaskManagement.DataAccess
{
    public static class TaskManagementDataAccessExtension
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddRedisDbContext(configuration);
            services.AddScoped<ITaskManagementRepository, TaskManagementRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            return services;
        }
    }
}
