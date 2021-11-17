using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class Boat : BaseModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int Capacity { get; set; }
        public string EngineNumber { get; set; }
        public int EnginePower { get; set; }
        public int MaxSpeed { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CancelationPolicyId { get; set; }
        public string Description { get; set; } //300 characters max
        public double AverageGrade { get; set; }
        public double RegularPrice { get; set; }
        public double WeekendPrice { get; set; }
        public string BoatOwnerId { get; set; }

    }
}
