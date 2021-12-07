using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Models
{
    public class SystemOptions : BaseModel
    {
        public string OptionName { get; set; }
        public string OptionValue { get; set; }
    }
}
