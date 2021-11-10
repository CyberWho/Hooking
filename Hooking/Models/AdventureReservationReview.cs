using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class AdventureReservationReview : BaseModel
    {
        public string AdventureReservationId { get; set; }
        public string Review { get; set; } // limited to 300 characters
        public bool DidntShow { get; set; }
        public bool ReceivedPenalty { get; set; }
    }
}
