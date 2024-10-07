using BeymenCase.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeymenCase.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> CreateAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
        Task DeleteAsync(string id);
    }
}
