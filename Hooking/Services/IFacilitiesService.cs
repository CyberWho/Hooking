using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface IFacilitiesService
    {
        Facilities GetById(Guid id);
        Facilities GetByCottageId(Guid cottageId);
        bool Create(Guid id, Facilities facilities);
        bool DeleteById(Guid id);
        bool AddToCottage(CottagesFacilities cottagesFacilities);
        Facilities Update(Guid id, Facilities facilities);
        CottagesFacilities GetByFacilitiesId(Guid id);
    }
}
