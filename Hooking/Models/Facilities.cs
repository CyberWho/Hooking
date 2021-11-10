using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class Facilities : BaseModel
    {
        public bool Parking { get; set; }
        public bool Wifi { get; set; }
        public bool Heating { get; set; }
        public bool BarbecueFacilities { get; set; }
        public bool OnlineCheckin { get; set; }
        public bool Jacuzzi { get; set; }
        public bool SeaView { get; set; }
        public bool MountainView { get; set; }
        public bool Kitchen { get; set; }
        public bool WashingMachine { get; set; }
        public bool AirportShuttle { get; set; }
        public bool IndoorPool { get; set; }
        public bool OutdoorPool { get; set; }
        public bool StockedBar { get; set; }
        public bool Garden { get; set; }
    }
}
