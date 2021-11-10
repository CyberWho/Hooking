using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public enum DeletionType { USER, INSTRUCTOR, BOATOWNER, COTTAGEOWNER, ADMIN }
    public class UserDeleteRequest : BaseModel
    {
        public string UserDetailsId { get; set; }
        public DateTime CreationTime { get; } = DateTime.Now;
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public DeletionType Type { get; set; } 
     

    }
}
