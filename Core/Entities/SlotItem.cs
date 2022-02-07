using ParkingSystem.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Entities
{
    public class SlotItem : Entity
    {
        public int SlotNo { get; set; }
        public bool Used { get; set; }
    }
}
