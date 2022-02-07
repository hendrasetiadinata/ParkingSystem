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
    public class SlotRepo : ParkingSystemRepositoryBase<ParkingContext, Slot>, ISlotRepo
    {
        public SlotRepo(ParkingContext dbContext) : base(dbContext)
        {
        }

        public override async Task<bool> DeleteAsync(Slot entity, CancellationToken token = default)
        {
            return await base.DeleteAsync(entity, token);
        }

        public override async Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default)
        {
            return await base.ExecuteCommandAsync(query, parameters, token);
        }

        public override async Task<Slot> GetAsync(CancellationToken token = default)
        {
            return await base.GetAsync(token);
        }

        public override async Task<Slot> GetAsync(long id, CancellationToken token = default)
        {
            return await base.GetAsync(id, token);
        }

        public override async Task<IEnumerable<Slot>> GetListAsync(CancellationToken token = default)
        {
            return await base.GetListAsync(token);
        }

        public override async Task<Slot> InsertAsync(Slot entity, CancellationToken token = default)
        {
            var domain = new Slot()
            {
                Active = entity.Active,
                Available = entity.Available,
                CreatedTime = DateTime.Now,
                Total = entity.Total,
                Used = entity.Used
            };
            return await base.InsertAsync(domain, token);
        }

        public override async Task<bool> UpdateAsync(Slot entity, CancellationToken token = default)
        {
            var domain = await _dbContext.Slots.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: token);
            if (domain == null) return false;

            domain.LastUpdatedTime = DateTime.Now;
            domain.Total = entity.Total;
            domain.Used = entity.Used;
            domain.Active = entity.Active;
            domain.Available = entity.Available;
            return await base.UpdateAsync(domain, token);
        }

        public async Task<bool> CreateParkingLot(int size)
        {
            try
            {
                var domain = await _dbContext.Slots.FirstOrDefaultAsync();
                int lastSlotNo = 0;

                if (domain != null)
                {
                    lastSlotNo = domain.Total;
                    domain.LastUpdatedTime = DateTime.Now;
                    domain.Total += size;
                    domain.Available += size;
                    _dbContext.Slots.Update(domain);
                }
                else
                {
                    await _dbContext.Slots.AddAsync(new Slot()
                    {
                        Active = true,
                        Available = size,
                        CreatedTime = DateTime.Now,
                        Total = size,
                        Used = 0,
                        ParkingFeePerHour = 2000
                    });
                }

                var slotItem = new List<SlotItem>();
                for (int i = 1; i <= size; i++)
                {
                    slotItem.Add(new SlotItem()
                    {
                        CreatedTime = DateTime.Now,
                        SlotNo = lastSlotNo + i,
                        Used = false,
                    });
                }
                await _dbContext.SlotItems.AddRangeAsync(slotItem);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    
    }
}
