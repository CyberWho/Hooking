using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class Cottage : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; } // limited ?
        public int RoomCount { get; set; }
        public int Area { get; set; }
        public double AverageGrade { get; set; }
        public int GradeCount { get; set; }
        public string CancelationPolicyId { get; set; }

        // FreeParking, FreeWifi, Heating, OnlineCheckIn

        // CottageRoom (N) <------> (N) Cottage

        // 1, 3
        // 1, 2
        // 1 TV, Bedroom, AirCondition, 2, 1
        // 2 TV, Bedroom, 2 kreveta
        // 3 TV, Bedroom, AirCondition, 2, 3
    }
}
