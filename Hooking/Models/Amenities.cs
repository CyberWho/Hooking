using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class Amenities : BaseModel
    {

        public bool Gps { get; set; }
        public bool Radar { get; set; }
        public bool VhrRadio { get; set; }
        public bool Sonar { get; set; }
        public bool FishFinder { get; set; }
        public bool WiFi { get; set; }
    }
}
