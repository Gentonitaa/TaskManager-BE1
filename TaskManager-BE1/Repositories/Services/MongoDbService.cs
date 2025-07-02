using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string LogsCollectionName { get; set; }
    }

    public class MongoDbService
    {
        private readonly IMongoCollection<UserLog> _logsCollection;

        public MongoDbService(IOptions<MongoDBSettings> mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
            _logsCollection = mongoDatabase.GetCollection<UserLog>(mongoSettings.Value.LogsCollectionName);
        }

        public async Task CreateLogAsync(UserLog log)
        {
            await _logsCollection.InsertOneAsync(log);
        }

        public async Task<List<UserLog>> GetAllLogsAsync()
        {
            return await _logsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<UserLog>> GetFilteredLogsAsync(string? userId, string? action)
        {
            var filterBuilder = Builders<UserLog>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                filter &= filterBuilder.Eq(log => log.UserId, userId);
            }

            if (!string.IsNullOrEmpty(action))
            {
                filter &= filterBuilder.Eq(log => log.Action, action);
            }

            return await _logsCollection.Find(filter).ToListAsync();
        }
    }
}
