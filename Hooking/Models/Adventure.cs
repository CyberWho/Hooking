using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Range(1, int.MaxValue, ErrorMessage = "Molimo unesite pozitivan broj")]
        public int MaxPersonCount { get; set; }
        public string CancellationPolicyId { get; set; }
        public double AverageGrade { get; set; }
    }
}
