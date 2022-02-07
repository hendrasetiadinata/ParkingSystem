using ParkingSystem.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Entities
{
    public class ParkingOrder : Entity
    {
        public int SlotNo { get; set; }
        public string LicensePlate { get; set; }
        public string Vehicle { get; set; }
        public string Colour { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? TotalMinutes { get; set; }
        public decimal ParkingFeePerHour { get; set; }
        public decimal? ParkingFee { get; set; }
    }
}
