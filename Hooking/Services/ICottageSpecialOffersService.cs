using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageSpecialOffersService
    {
        CottageSpecialOffer GetById(Guid id);
        bool Create(CottageSpecialOffer cottageSpecialOffer);
        bool DeleteById(Guid id);
        CottageSpecialOffer Update(Guid id, CottageSpecialOffer cottageSpecialOffer);
    }
}
