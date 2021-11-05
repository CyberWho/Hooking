using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Hooking.Models
{
    public class CancelationPolicy : BaseModel
    {
        public int FreeUntil { get; set; }
        public int PenaltyPercentage { get; set; }
    }
}
