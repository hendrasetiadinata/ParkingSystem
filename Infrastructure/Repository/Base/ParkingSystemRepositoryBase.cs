using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.Entities.Base;
using ParkingSystem.Core.Model;
using ParkingSystem.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingSystem.Infrastructure.Repository.Base
{
    public class ParkingSystemRepositoryBase<D, T> : IRepository<T>
       where D : DbContext
       where T : Entity
    {
        protected readonly D _dbContext;

        public ParkingSystemRepositoryBase(D dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> InsertAsync(T entity, CancellationToken token = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken: token);
            await _dbContext.SaveChangesAsync(cancellationToken: token);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(T entity, CancellationToken token = default)
        {
            _dbContext.Set<T>().Remove(entity);
            var result = await _dbContext.SaveChangesAsync(cancellationToken: token);
            return result > 0;
        }

        public virtual async Task<bool> ExecuteCommandAsync(string query, SqlParameter[] parameters = null, CancellationToken token = default)
        {
            var execute = await _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
            return execute > 0;
        }

        public virtual async Task<T> GetAsync(CancellationToken token = default)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(cancellationToken: token);
        }

        public virtual async Task<T> GetAsync(long id, CancellationToken token = default)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: token);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(CancellationToken token = default)
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken: token);
        }

        public virtual async Task<bool> UpdateAsync(T entity, CancellationToken token = default)
        {
            _dbContext.Set<T>().Update(entity);
            var update = await _dbContext.SaveChangesAsync(cancellationToken: token);
            return update > 0;
        }
    }
}
