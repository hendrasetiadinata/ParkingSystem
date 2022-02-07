using ParkingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Interfaces
{
    public interface IParkingServices
    {
        Task<string> Create_parking_lot(int size);
        Task<string> Park(string plateNo, string colour, string vehicle);
        Task<string> Leave(int slotNo);
        Task<IEnumerable<ParkingOrder>> Status();
        Task<string> Type_of_vehicles(string vehicle);
        Task<string> Registration_numbers_for_vehicles_with_ood_plate();
        Task<string> Registration_numbers_for_vehicles_with_event_plate();
        Task<string> Registration_numbers_for_vehicles_with_colour(string colour);
        Task<string[]> Slot_numbers_for_vehicles_with_colour(string colour);
        Task<string> Slot_number_for_registration_number(string platNo);
        Task<IEnumerable<SlotItem>> GetSlotItem();
    }
}
