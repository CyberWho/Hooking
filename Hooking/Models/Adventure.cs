using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class Adventure : BaseModel
    {
        public string InstructorId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int MaxPersonCount { get; set; }
        public string CancelationPolicy { get; set; }
        public double AverageGrade { get; set; }
    }
}
