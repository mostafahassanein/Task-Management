using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Entities;

namespace TaskManagement.DataAccess.Interfaces
{
    public interface ITaskManagementRepository
    {
        Task<string?> UpdateTask(Tasks tasks);
        Task<List<Tasks>> GetAllsTasks();
        public Task<Tasks> GetTask(string taskId);
        public Task<string?> RemoveTask(string taskId);
    }
}
