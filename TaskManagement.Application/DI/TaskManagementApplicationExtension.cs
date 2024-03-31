using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Implementations;
using TaskManagement.Application.Interfaces;
using TaskManagement.DataAccess;

namespace TaskManagement.Application
{
    public static class TaskManagementApplicationExtension
    {
        public static IServiceCollection AddTaskManagementApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDataAccessServices(configuration);

            services.AddScoped<ITaskManagementService, TaskManagementService>();
            services.AddScoped<IChatHistoryService, ChatHistoryService>();

            return services;
        }
    }
}
