using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Entities.Base
{
    public abstract class Entity : EntityBase<long>
    {
        public DateTime CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }
}
