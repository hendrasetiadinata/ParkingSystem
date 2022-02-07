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
    public interface ISlotRepo
    {
        Task<bool> DeleteAsync(Slot entity, CancellationToken token = default);
        Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default);
        Task<Slot> GetAsync(CancellationToken token = default);
        Task<Slot> GetAsync(long id, CancellationToken token = default);
        Task<IEnumerable<Slot>> GetListAsync(CancellationToken token = default);
        Task<Slot> InsertAsync(Slot entity, CancellationToken token = default);
        Task<bool> UpdateAsync(Slot entity, CancellationToken token = default);
        Task<bool> CreateParkingLot(int size);
    }
}
