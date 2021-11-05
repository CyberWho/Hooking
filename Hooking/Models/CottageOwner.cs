using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class CottageOwner : BaseModel
    {
        public string UserDetailsId { get; set; }
        public double AverageGrade { get; set; }
        public int GradeCount { get; set; }

    }
}
