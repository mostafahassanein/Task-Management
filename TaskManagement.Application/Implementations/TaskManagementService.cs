using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.DataAccess.Interfaces;
using TaskManagement.Entities;

namespace TaskManagement.Application.Implementations
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly ITaskManagementRepository _taskManagementRepository;
        public TaskManagementService(ITaskManagementRepository taskManagementRepository)
        {
            _taskManagementRepository = taskManagementRepository;
        }
        public async Task<Tasks> GetTask(string taskId)
        {
            return await _taskManagementRepository.GetTask(taskId);
        }
        public async Task<string?> UpdateTask(Tasks tasks)
        {
            return await _taskManagementRepository.UpdateTask(tasks);
        }

        public async Task<List<Tasks>> GetAllsTasks()
        {
            return await _taskManagementRepository.GetAllsTasks();
        }
        public async Task<string?> RemoveTask(string taskId)
        {
            return await _taskManagementRepository.RemoveTask(taskId);
        }
    }
}
