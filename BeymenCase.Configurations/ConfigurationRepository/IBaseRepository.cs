using BeymenCase.Configurations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeymenCase.Configurations.ConfigurationRepository
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> CreateAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
        Task DeleteAsync(string id);
    }
}
