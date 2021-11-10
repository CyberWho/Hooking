using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class FishingEquipment : BaseModel
    {
        public bool LiveBite { get; set; }
        public bool FlyFishingGear { get; set; }
        public bool Lures { get; set; }
        public bool RodsReelsTackle { get; set; }


    }
}
