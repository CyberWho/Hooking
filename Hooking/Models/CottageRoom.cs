using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class CottageRoom : BaseModel
    {
        public List<string> CottageIds { get; set; }
        public int BedCount { get; set; }
        public bool AirCondition { get; set; }
        public bool TV { get; set; }
        public bool Balcony { get; set; }
    }
}
