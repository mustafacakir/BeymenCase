using BeymenCase.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeymenCase.Core.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<ConfigurationRecord>> GetActiveConfigurationsAsync(string applicationName);
    }
}
