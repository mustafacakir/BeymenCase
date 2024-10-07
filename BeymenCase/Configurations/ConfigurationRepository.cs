using BeymenCase.Core.Entities;
using BeymenCase.Core.Interfaces;
using BeymenCase.Infrastructure.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeymenCase.Infrastructure.Configurations
{
    public class ConfigurationRepository : RedisRepository<ConfigurationRecord>, IConfigurationRepository
    {
        public ConfigurationRepository(string connectionString) : base(connectionString){}


        public async Task<IEnumerable<ConfigurationRecord>> GetActiveConfigurationsAsync(string applicationName)
        {
            var keys = _database.Multiplexer.GetServer(_database.Multiplexer.Configuration).Keys(pattern: $"{applicationName}:*");
            var tasks = keys.Select(key => GetByIdAsync(key));
            var configurations = await Task.WhenAll(tasks);
            return configurations.Where(c => c != null && c.IsActive);
        }
    }
}
