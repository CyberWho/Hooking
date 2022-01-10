using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class CottageAppeal: BaseModel
    {
        public string CottageId { get; set; }
        [DisplayName("Datum objave")]
        public DateTime CreationTime { get; } = DateTime.Now;
        [DisplayName("Zalba")]
        public string AppealContent { get; set; } // limited to 300 chars
        public string Email { get; set; }
    }
}
