using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class CottageOwnerReview : BaseModel
    {
        public string CottageOwnerId { get; set; }
        public string UserDetailsId { get; set; }
        public DateTime CreationDate { get; } = DateTime.Now;
        public string Review { get; set; } // limited to 300 characters
        public int Grade { get; set; }
        public bool IsApproved { get; set; }
    }
}
