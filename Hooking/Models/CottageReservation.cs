using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class CottageReservation : BaseModel
    {
        public string CottageId { get; set; }
        public string UserDetailsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int PersonCount { get; set; }
    }
}
