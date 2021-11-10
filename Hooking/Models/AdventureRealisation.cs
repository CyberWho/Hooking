using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class AdventureRealisation : BaseModel
    {
        public string AdventureId { get; set; }
        public double Duration { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        

    }
}
