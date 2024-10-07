using BeymenCase.Core.Entities;
using BeymenCase.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeymenCase.Applications.Services
{
    public class ConfigurationReader : IConfigurationReaderCore
    {
        private readonly ConfigurationRepository _repository;
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        private IEnumerable<Configuration> _cachedConfigurations;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim(); 

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _repository = new ConfigurationRepository(connectionString);
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => StartRefreshing(_cancellationTokenSource.Token));
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            await RefreshConfigurations();

            _cacheLock.EnterReadLock();
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
                _cacheLock.ExitReadLock();
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
                Console.WriteLine($"Konfigürasyonları yenilerken hata oluştu: {ex.Message}");
            }

            if (configurations != null && configurations.Any())
            {
                _cacheLock.EnterWriteLock();
                try
                {
                    _cachedConfigurations = configurations;
                }
                finally
                {
                    _cacheLock.ExitWriteLock(); 
                }
            }
        }

        public async Task StartRefreshing(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RefreshConfigurations();
                await Task.Delay(_refreshTimerIntervalInMs, cancellationToken);
            }
        }
    }
}
