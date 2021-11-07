using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class HouseRules : BaseModel
    {
        public bool PetFriendly { get; set; }
        public bool NonSmoking { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
        public int AgeRestriction { get; set; }
    }
}
