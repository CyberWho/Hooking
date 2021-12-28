using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageNotAvailablePeriodsService
    {
        IEnumerable<CottageNotAvailablePeriod> GetAllByCottageId(Guid cottageId);
        CottageNotAvailablePeriod GetById(Guid id);
        bool Create(CottageNotAvailablePeriod cottageNotAvailablePeriod);
    }
}
