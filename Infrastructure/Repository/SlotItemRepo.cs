using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.Entities;
using ParkingSystem.Core.Interfaces;
using ParkingSystem.Infrastructure.Repository.Base;

namespace ParkingSystem.Infrastructure.Repository
{

    public class SlotItemRepo : ParkingSystemRepositoryBase<ParkingContext, SlotItem>, ISlotItemRepo
    {
        public SlotItemRepo(ParkingContext dbContext) : base(dbContext)
        {
        }

        public override async Task<bool> DeleteAsync(SlotItem entity, CancellationToken token = default)
        {
            return await base.DeleteAsync(entity, token);
        }

        public override async Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default)
        {
            return await base.ExecuteCommandAsync(query, parameters, token);
        }

        public override async Task<SlotItem> GetAsync(CancellationToken token = default)
        {
            return await base.GetAsync(token);
        }

        public override async Task<SlotItem> GetAsync(long id, CancellationToken token = default)
        {
            return await base.GetAsync(id, token);
        }

        public override async Task<IEnumerable<SlotItem>> GetListAsync(CancellationToken token = default)
        {
            return await base.GetListAsync(token);
        }

        public override async Task<SlotItem> InsertAsync(SlotItem entity, CancellationToken token = default)
        {
            var domain = new SlotItem()
            {
                CreatedTime = DateTime.Now,
                SlotNo = entity.SlotNo,
                Used = entity.Used
            };
            return await base.InsertAsync(domain, token);
        }

        public override async Task<bool> UpdateAsync(SlotItem entity, CancellationToken token = default)
        {
            var domain = await _dbContext.SlotItems.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: token);
            if (domain == null) return false;

            domain.LastUpdatedTime = DateTime.Now;
            domain.Used = entity.Used;
            domain.SlotNo = entity.SlotNo;
            return await base.UpdateAsync(domain, token);
        }
    }
}
