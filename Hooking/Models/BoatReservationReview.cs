using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class BoatReservationReview : BaseModel
    {
        public string BoatReservationId { get; set; }
        public string Review { get; set; } // limited to 300 characters
        public bool DidntShow { get; set; }
        public bool ReceivedPenalty { get; set; }
    }
}
