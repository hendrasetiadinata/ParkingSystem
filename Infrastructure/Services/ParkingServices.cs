using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingSystem.Core.Entities;
using ParkingSystem.Core.Interfaces;

namespace ParkingSystem.Infrastructure.Services
{
    public class ParkingServices : IParkingServices
    {
        private readonly IParkingOrderRepo _parkingOrderRepo;
        private readonly ISlotRepo _sloteRepo;
        private readonly ISlotItemRepo _slotItemRepo;

        public ParkingServices(IParkingOrderRepo parkingOrderRepo, ISlotRepo slotRepo, ISlotItemRepo slotItemRepo)
        {
            _parkingOrderRepo = parkingOrderRepo;
            _sloteRepo = slotRepo;
            _slotItemRepo = slotItemRepo;
        }
        
        public async Task<string> Create_parking_lot(int size)
        {
            var status = await _sloteRepo.CreateParkingLot(size);
            if (status) return $"Created a parking lot with {size} slots";
            return $"Created a parking lot is fail";
        }

        public async Task<string> Leave(int slotNo)
        {
            var order = await _parkingOrderRepo.LeaveAsync(slotNo);
            if (order != null) return $"Slot number {order.SlotNo} is free";
            return $"Leave park is fail";
        }

        public async Task<string> Park(string plateNo, string colour, string vehicle)
        {
            var domain = new ParkingOrder()
            {
                LicensePlate = plateNo,
                Colour = colour,
                Vehicle = vehicle,
                CheckIn = DateTime.Now,
                CreatedTime = DateTime.Now
            };

            try
            {
                var order = await _parkingOrderRepo.InsertAsync(domain);
                if (order != null) return $"Allocated slot number: {order.SlotNo}";
                return $"Park is fail";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> Registration_numbers_for_vehicles_with_colour(string colour)
        {
            var data = await _parkingOrderRepo.Registration_numbers_for_vehicles_with_colour(colour);
            if (!string.IsNullOrEmpty(data)) return data;
            return "Not found";
        }

        public async Task<string> Registration_numbers_for_vehicles_with_event_plate()
        {
            var data = await _parkingOrderRepo.Registration_numbers_for_vehicles_with_odd_even_plate(false);
            if (!string.IsNullOrEmpty(data)) return data;
            return "Not found";
        }

        public async Task<string> Registration_numbers_for_vehicles_with_ood_plate()
        {
            var data = await _parkingOrderRepo.Registration_numbers_for_vehicles_with_odd_even_plate(true);
            if (!string.IsNullOrEmpty(data)) return data;
            return "Not found";
        }

        public async Task<string[]> Slot_numbers_for_vehicles_with_colour(string colour)
        {
            var data = await _parkingOrderRepo.Slot_numbers_for_vehicles_with_colour(colour);
            if (data.Any()) return data;
            return new string[] { "Not found" };
        }

        public async Task<string> Slot_number_for_registration_number(string platNo)
        {
            var data = await _parkingOrderRepo.Slot_number_for_registration_number(platNo);
            if (data.Any()) return data;
            return "Not found";
        }

        public async Task<IEnumerable<ParkingOrder>> Status()
        {
            var data = await _parkingOrderRepo.GetListAsync();
            return data;
        }

        public async Task<string> Type_of_vehicles(string vehicle)
        {
            var count = await _parkingOrderRepo.Type_of_vehicles(vehicle);
            return count.ToString();
        }

        public async Task<IEnumerable<SlotItem>> GetSlotItem()
        {
            var data = await _slotItemRepo.GetListAsync();
            return data;
        }
    }
}
