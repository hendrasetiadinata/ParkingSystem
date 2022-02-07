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
    public interface IParkingOrderRepo
    {
        Task<bool> DeleteAsync(ParkingOrder entity, CancellationToken token = default);
        Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default);
        Task<ParkingOrder> GetAsync(CancellationToken token = default);
        Task<ParkingOrder> GetAsync(long id, CancellationToken token = default);
        Task<IEnumerable<ParkingOrder>> GetListAsync(CancellationToken token = default);
        Task<ParkingOrder> InsertAsync(ParkingOrder entity, CancellationToken token = default);
        Task<bool> UpdateAsync(ParkingOrder entity, CancellationToken token = default);
        Task<ParkingOrder> LeaveAsync(int slotNo, CancellationToken token = default);
        Task<string> Registration_numbers_for_vehicles_with_odd_even_plate(bool isOdd, CancellationToken token = default);
        Task<string> Registration_numbers_for_vehicles_with_colour(string colour, CancellationToken token = default);
        Task<string[]> Slot_numbers_for_vehicles_with_colour(string colour, CancellationToken token = default);
        Task<string> Slot_number_for_registration_number(string platNo, CancellationToken token = default);
        Task<string> Type_of_vehicles(string vehicle, CancellationToken token = default);
    }
}
