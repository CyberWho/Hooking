using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class BoatOwner : BaseModel
    {
        public string UserDetailsId { get; set; }
        public double AverageGrade { get; set; }
        public int GradeCount { get; set; }
        public bool IsCaptain { get; set; }
        public bool IsFirstOfficer { get; set; }
    }
}
