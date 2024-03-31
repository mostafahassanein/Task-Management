using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Entities;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskManagementService
    {
        Task<string?> UpdateTask(Tasks tasks);
        Task<List<Tasks>> GetAllsTasks();
        Task<Tasks> GetTask(string taskId);
        Task<string?> RemoveTask(string taskId);
    }
}
