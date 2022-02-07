using ParkingSystem.Core.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Threading;

namespace ParkingSystem.Core.Repository.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<IEnumerable<T>> GetListAsync(CancellationToken token = default);
        Task<T> GetAsync(CancellationToken token = default);
        Task<T> GetAsync(long id, CancellationToken token = default);
        Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default);
        Task<T> InsertAsync(T entity, CancellationToken token = default);
        Task<bool> UpdateAsync(T entity, CancellationToken token = default);
        Task<bool> DeleteAsync(T entity, CancellationToken token = default);
    }
}
