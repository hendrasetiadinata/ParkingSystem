using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.Entities;
using ParkingSystem.Core.Interfaces;
using ParkingSystem.Infrastructure.Repository.Base;

namespace ParkingSystem.Infrastructure.Repository
{
    
    public class ParkingOrderRepo : ParkingSystemRepositoryBase<ParkingContext, ParkingOrder>, IParkingOrderRepo
    {
        public ParkingOrderRepo(ParkingContext dbContext) : base(dbContext)
        {
        }

        public override async Task<bool> DeleteAsync(ParkingOrder entity, CancellationToken token = default)
        {
            return await base.DeleteAsync(entity, token);
        }

        public override async Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default)
        {
            return await base.ExecuteCommandAsync(query, parameters, token);
        }

        public async Task<string> Registration_numbers_for_vehicles_with_odd_even_plate(bool isOdd, CancellationToken token = default)
        {
            var data = await _dbContext.ParkingOrders.AsNoTracking().Where(x => x.CheckOut == null)
                   .Select(s => s.LicensePlate ).ToArrayAsync(cancellationToken: token);
            
            var oddList = new List<string>();
            foreach (var item in data)
            {
                var number = item.Split('-')[1];
                if (int.TryParse(number, out int platNo))
                {
                    if (isOdd && platNo % 2 != 0)
                    {
                        oddList.Add(item);
                    }
                    else if (!isOdd && platNo % 2 == 0)
                    {
                        oddList.Add(item);
                    }
                }
            }
            return string.Join(",", oddList.Distinct());
        }

        public async Task<string> Registration_numbers_for_vehicles_with_colour(string colour, CancellationToken token = default)
        {
            var data = await _dbContext.ParkingOrders.AsNoTracking().Where(x => x.Colour.ToLower() == colour.ToLower() && x.CheckOut == null)
                .Select(s => s.LicensePlate).ToArrayAsync(cancellationToken: token);
            return string.Join(",", data);
        }

        public async Task<string[]> Slot_numbers_for_vehicles_with_colour(string colour, CancellationToken token = default)
        {
            var data = await _dbContext.ParkingOrders.AsNoTracking().Where(x => x.Colour.ToLower() == colour.ToLower() && x.CheckOut == null)
                .Select(s => s.SlotNo).ToArrayAsync(cancellationToken: token);
            return new string[] { string.Join(",", data), data.Length.ToString() };
        }

        public async Task<string> Slot_number_for_registration_number(string platNo, CancellationToken token = default)
        {
            var data = await _dbContext.ParkingOrders.AsNoTracking().Where(x => x.LicensePlate.ToLower() == platNo.ToLower() && x.CheckOut == null)
                .Select(s => s.SlotNo).ToArrayAsync(cancellationToken: token);
            return string.Join(",", data);
        }

        public override async Task<ParkingOrder> GetAsync(CancellationToken token = default)
        {
            return await base.GetAsync(token);
        }

        public override async Task<ParkingOrder> GetAsync(long id, CancellationToken token = default)
        {
            return await base.GetAsync(id, token);
        }

        public override async Task<IEnumerable<ParkingOrder>> GetListAsync(CancellationToken token = default)
        {
            return await base.GetListAsync(token);
        }

        public override async Task<ParkingOrder> InsertAsync(ParkingOrder entity, CancellationToken token = default)
        {
            //var transaction = await _dbContext.Database.BeginTransactionAsync(token);

            try
            {
                var slot = await _dbContext.Slots.AsNoTracking().FirstOrDefaultAsync(cancellationToken: token);
                var availSlot = await _dbContext.SlotItems.AsTracking().FirstOrDefaultAsync(x => x.Used == false, cancellationToken: token);
                if (availSlot == null)
                {
                    //await transaction.RollbackAsync(token);
                    throw new Exception("Sorry, parking lot is full");
                }

                availSlot.Used = true;
                availSlot.LastUpdatedTime = DateTime.Now;
                _dbContext.SlotItems.Update(availSlot);

                var domain = new ParkingOrder()
                {
                    CheckIn = entity.CheckIn,
                    CreatedTime = entity.CreatedTime,
                    LicensePlate = entity.LicensePlate,
                    ParkingFeePerHour = slot.ParkingFeePerHour,
                    Colour = entity.Colour,
                    Vehicle = entity.Vehicle,
                    SlotNo = availSlot.SlotNo
                };
                await _dbContext.ParkingOrders.AddAsync(domain, token);                
                var result = await _dbContext.SaveChangesAsync(token);

                if (result > 0)
                {
                    //transaction.Commit();
                    return domain;
                }
                else
                {
                    //transaction.Rollback();
                    return null;
                }
            }
            catch (Exception)
            {
                //await transaction.RollbackAsync(token);
                throw;
            }
        }

        public async Task<ParkingOrder> LeaveAsync(int slotNo, CancellationToken token = default)
        {
            //var transaction = await _dbContext.Database.BeginTransactionAsync(token);

            try
            {
                var slotItem = await _dbContext.SlotItems.AsTracking().FirstOrDefaultAsync(x => x.SlotNo == slotNo, cancellationToken: token);
                slotItem.Used = false;
                slotItem.LastUpdatedTime = DateTime.Now;

                var order = await _dbContext.ParkingOrders.AsTracking().FirstOrDefaultAsync(x => x.SlotNo == slotNo, token);
                order.CheckOut = DateTime.Now;
                order.LastUpdatedTime = DateTime.Now;
                var timeSpan = order.CheckIn - order.CheckOut.Value;
                order.TotalMinutes = Convert.ToInt32(timeSpan.TotalHours);
                order.ParkingFee = order.ParkingFeePerHour * order.TotalMinutes.Value;
                _dbContext.ParkingOrders.Update(order);

                var result = await _dbContext.SaveChangesAsync(token);

                if (result > 0)
                {
                    //transaction.Commit();
                    return order;
                }
                else
                {
                    //transaction.Rollback();
                    return null;
                }
            }
            catch (Exception)
            {
                //await transaction.RollbackAsync(token);
                throw;
            }
        }

        public override async Task<bool> UpdateAsync(ParkingOrder entity, CancellationToken token = default)
        {
            var domain = await _dbContext.ParkingOrders.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: token);
            if (domain == null) return false;

            domain.LastUpdatedTime = DateTime.Now;
            domain.CheckIn = entity.CheckIn;
            domain.CheckOut = entity.CheckOut;
            domain.LicensePlate = entity.LicensePlate;
            domain.ParkingFee = entity.ParkingFee;
            domain.ParkingFeePerHour = entity.ParkingFeePerHour;
            domain.TotalMinutes = entity.TotalMinutes;
            domain.Colour = entity.Colour;
            domain.Vehicle = entity.Vehicle;
            return await base.UpdateAsync(domain, token);
        }
    
        public async Task<string> Type_of_vehicles(string vehicle, CancellationToken token = default)
        {
            var data = await _dbContext.ParkingOrders.AsNoTracking().Where(x => x.Vehicle.ToLower() == vehicle.ToLower() && x.CheckOut == null)
                .CountAsync(token);
            return data.ToString();
        }
    }
}
