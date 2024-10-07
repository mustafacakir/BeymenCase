using System.Threading.Tasks;

namespace BeymenCase.Configurations.ConfigurationReader
{
    public interface IConfigurationReader
    {
        Task<T> GetValueAsync<T>(string key);
    }
}
