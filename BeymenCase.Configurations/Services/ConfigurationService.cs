using BeymenCase.Configurations.ConfigurationRepository;
using BeymenCase.Configurations.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeymenCase.Configurations.Services
{
    public class ConfigurationService : RedisRepository<Configuration>, IConfigurationService
    {
        public ConfigurationService(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Configuration>> GetActiveConfigurationsAsync(string applicationName)
        {
            var keys = _database.Multiplexer.GetServer(_database.Multiplexer.Configuration).Keys(pattern: $"{applicationName}:*");
            var tasks = keys.Select(key => GetByIdAsync(key));
            var configurations = await Task.WhenAll(tasks);
            return configurations.Where(c => c != null && c.IsActive);
        }
    }
}
