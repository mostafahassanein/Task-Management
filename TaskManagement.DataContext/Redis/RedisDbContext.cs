using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DataContext
{
    public class RedisDbContext
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _db;
        private readonly IDatabase _dbChat;
        private readonly IServer _server;
        public RedisDbContext(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisConnection = connectionMultiplexer;
            _db = _redisConnection.GetDatabase();
            _dbChat = _redisConnection.GetDatabase(1);
            _server = _redisConnection.GetServers().First();
        }

        public IDatabase Database { get { return _db; } }
        public IDatabase DatabaseChat { get { return _dbChat; } }
        public IServer Server { get { return _server; } }
    }
}
