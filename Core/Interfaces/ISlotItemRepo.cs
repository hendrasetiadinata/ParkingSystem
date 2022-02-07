using Microsoft.Data.SqlClient;
using ParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Interfaces
{

    public interface ISlotItemRepo
    {
        Task<bool> DeleteAsync(SlotItem entity, CancellationToken token = default);
        Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default);
        Task<SlotItem> GetAsync(CancellationToken token = default);
        Task<SlotItem> GetAsync(long id, CancellationToken token = default);
        Task<IEnumerable<SlotItem>> GetListAsync(CancellationToken token = default);
        Task<SlotItem> InsertAsync(SlotItem entity, CancellationToken token = default);
        Task<bool> UpdateAsync(SlotItem entity, CancellationToken token = default);
    }
}
