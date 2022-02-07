using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Entities.Base
{
    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        public virtual TId Id { get; set; }
    }
}
