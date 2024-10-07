using BeymenCase.Configurations.Models;
using BeymenCase.Configurations.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeymenCase.Configurations.ConfigurationReader
{
    public class ConfigurationReader : BackgroundService, IConfigurationReader
    {
        private readonly ConfigurationService _repository;
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        private IEnumerable<Configuration> _cachedConfigurations;
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _repository = new ConfigurationService(connectionString);
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            await RefreshConfigurations();

            await _cacheLock.WaitAsync();
            try
            {
                var config = _cachedConfigurations?.FirstOrDefault(c => c.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

                if (config?.Value == null)
                {
                    return default;
                }

                return (T)Convert.ChangeType(config.Value, typeof(T));
            }
            finally
            {
                _cacheLock.Release();
            }
        }

        private async Task RefreshConfigurations()
        {
            IEnumerable<Configuration> configurations = null;

            try
            {
                configurations = await _repository.GetActiveConfigurationsAsync(_applicationName);
            }
            catch (Exception ex)
            {
                //Logging
            }

            if (configurations != null && configurations.Any())
            {
                await _cacheLock.WaitAsync();
                try
                {
                    _cachedConfigurations = configurations;
                }
                finally
                {
                    _cacheLock.Release();
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RefreshConfigurations();
                await Task.Delay(_refreshTimerIntervalInMs, stoppingToken); 
            }
        }
    }
}
