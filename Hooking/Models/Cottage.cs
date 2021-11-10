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
        public double RegularPrice { get; set; }
        public double WeekendPrice { get; set; }

    }
}
