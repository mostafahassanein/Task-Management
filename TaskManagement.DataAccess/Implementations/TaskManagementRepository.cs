using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagement.DataAccess.Interfaces;
using TaskManagement.DataContext;
using TaskManagement.Entities;

namespace TaskManagement.DataAccess.Implementations
{
    public class TaskManagementRepository : ITaskManagementRepository
    {
        private readonly RedisDbContext _rdb;
        private readonly ILogger<TaskManagementRepository> _logger;
        public TaskManagementRepository(RedisDbContext rdb, ILogger<TaskManagementRepository> logger)
        {
            _rdb = rdb;
            _logger = logger;
        }
        public async Task<Tasks?> GetTask(string taskId)
        {
            try
            {
                return JsonSerializer.Deserialize<Tasks>(_rdb.Database.StringGet(taskId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
        public async Task<string?> UpdateTask(Tasks tasks)
        {
            try
            {
                string key = string.Empty;
                if (tasks.Id == 0) //add new record
                {
                    key = await GetNextId();
                    tasks.Id = int.Parse(key);
                }
                else //update
                {
                    key = tasks.Id.ToString();
                }
                
                if (_rdb.Database.StringSet(key, JsonSerializer.Serialize(tasks, (new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }))))//, flags: StackExchange.Redis.CommandFlags.FireAndForget);
                    return "Success";
                return "Failed";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in UpdateTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }

        public async Task<List<Tasks>> GetAllsTasks()
        {
            try
            {
                List<Tasks> list = new List<Tasks>();
                var values = _rdb.Database.StringGet(_rdb.Server.Keys().Cast<RedisKey>().ToArray());
                foreach (var item in values)
                {
                    list.Add(JsonSerializer.Deserialize<Tasks>(item));
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllsTasks. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
        public async Task<string?> RemoveTask(string taskId)
        {
            try
            {
                if (_rdb.Database.KeyDelete(taskId))
                    return "Success";
                return "Failed";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RemoveTask. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
        private async Task<string> GetNextId()
        {
            try
            {
                RedisKey[] keys = _rdb.Server.Keys(0).Cast<RedisKey>().ToArray();
                if (keys == null || keys.Length == 0)
                    return "1";
                //Array.Sort(keys);
                //string lastItem = keys[keys.Length - 1];
                //return (int.Parse(lastItem) + 1).ToString();
                return (keys.Count()+1).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetNextId. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
    }
}
