using BeymenCase.Core.Entities;
using BeymenCase.Core.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeymenCase.Infrastructure.Configuration
{
    public class RedisRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected IDatabase _database;

        public RedisRepository(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _database = redis.GetDatabase();
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            var server = _database.Multiplexer.GetServer(_database.Multiplexer.Configuration);
            var keys = server.Keys();

            var tasks = keys.Select(async key =>
            {
                var value = await _database.StringGetAsync(key);
                if (!value.IsNull)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                return null;
            });

            var items = await Task.WhenAll(tasks);
            return items.Where(c => c != null);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var key = id;
            var value = await _database.StringGetAsync(key);

            if (!value.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return null;
        }

        public async Task<T> CreateAsync(T model)
        {
            model.Id = Guid.NewGuid().ToString();
            var key = $"{model.ApplicationName}:{model.Id}";
            var value = JsonConvert.SerializeObject(model);
            await _database.StringSetAsync(key, value);

            return model;
        }

        public async Task UpdateAsync(T model)
        {
            var key = $"{model.ApplicationName}:{model.Id}";
            var existingValue = await _database.StringGetAsync(key);

            if (!existingValue.IsNullOrEmpty)
            {
                var existingModel = JsonConvert.DeserializeObject<T>(existingValue);
                if (existingModel != null)
                {
                    model.Id = existingModel.Id;
                    var updatedValue = JsonConvert.SerializeObject(model);
                    await _database.StringSetAsync(key, updatedValue);
                }
            }
        }

        public async Task DeleteAsync(T model)
        {
            var key = $"{typeof(T).Name}:{model.Id}";
            await _database.KeyDeleteAsync(key);
        }

        public async Task DeleteAsync(string id)
        {
            var key = $"{typeof(T).Name}:{id}";
            await _database.KeyDeleteAsync(key);
        }
    }
}
