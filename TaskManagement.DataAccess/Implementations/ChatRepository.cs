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
    public class ChatRepository : IChatRepository
    {
        private readonly RedisDbContext _rdb;
        private readonly ILogger<TaskManagementRepository> _logger;
        public ChatRepository(RedisDbContext rdb, ILogger<TaskManagementRepository> logger)
        {
            _rdb = rdb;
            _logger = logger;
        }

        public async Task<bool> AddMessage(ChatMessage chatMessage)
        {
            try
            {
                if (_rdb.DatabaseChat.StringSet(await GetNextId(), JsonSerializer.Serialize(chatMessage, (new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }))))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddMessage. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistory()
        {
            try
            {
                List<ChatMessage> list = new List<ChatMessage>();
                var values = _rdb.DatabaseChat.StringGet(_rdb.Server.Keys(1).Cast<RedisKey>().ToArray());
                foreach (var item in values)
                {
                    try
                    {
                        list.Add(JsonSerializer.Deserialize<ChatMessage>(item));
                    }
                    catch { continue; }
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetChatHistory. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
        private async Task<string> GetNextId()
        {
            try
            {
                RedisKey[] keys = _rdb.Server.Keys(1).Cast<RedisKey>().ToArray();
                if (keys == null || keys.Length == 0)
                    return "1";
                Array.Sort(keys);
                string lastItem = keys[keys.Length - 1];
                return (int.Parse(lastItem) + 1).ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetNextId. Timestamp: {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }
    }
}