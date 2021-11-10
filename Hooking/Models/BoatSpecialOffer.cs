using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class BoatSpecialOffer : BaseModel
    {
        public string BoatId { get; set; }
        public string UserDetailsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int MaxPersonCount { get; set; }
        public string Description { get; set; } // max 300 char
    }
}
