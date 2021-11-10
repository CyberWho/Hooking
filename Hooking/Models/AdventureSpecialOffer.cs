using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class AdventureSpecialOffer : BaseModel
    {
        public string AdventureRealisationId { get; set; }
        public string UserDetailsId { get; set; }
        public string Description { get; set; } // max 300 char
    }
}
