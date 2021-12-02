using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Areas.Identity.Pages.Account;

namespace Hooking.Models
{
    public class RegistrationRequest : BaseModel
    {
        public string UserDetailsId { get; set; }
        public RegistrationType Type { get; set; }
        public string Description { get; set; }
    }
}
