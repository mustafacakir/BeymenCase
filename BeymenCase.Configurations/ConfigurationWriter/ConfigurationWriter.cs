using BeymenCase.Configurations.ConfigurationRepository;
using BeymenCase.Configurations.Models;
using BeymenCase.Configurations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.Configurations.ConfigurationWriter
{
    public class ConfigurationWriter<T> : IConfigurationWriter<T>
    {
        private readonly ConfigurationService _repository;

        public ConfigurationWriter(string connectionString)
        {
            _repository = new ConfigurationService(connectionString);

        }

        public async Task<T> CreateAsync(T model)
        {
            return await CreateAsync(model);
        }

        public async Task DeleteAsync(T model)
        {
            await DeleteAsync(model);
        }

        public async Task DeleteAsync(string id)
        {
            await DeleteAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await GetListAsync();
        }

        public async Task UpdateAsync(T model)
        {
            await UpdateAsync(model);
        }
    }
}
