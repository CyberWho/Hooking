using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageOwnersService
    {
        CottageOwner GetById(Guid id);
        bool Create(CottageOwner cottageOwner);
        bool DeleteById(Guid id);
    }
}
