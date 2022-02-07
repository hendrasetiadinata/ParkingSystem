using ParkingSystem.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Entities
{
    public class Slot : Entity
    {
        public int Total { get; set; }
        public int Available { get; set; }
        public int Used { get; set; }
        public bool Active { get; set; }
        public decimal ParkingFeePerHour { get; set; }
    }
}
