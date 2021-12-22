using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageReservationsService
    {
        IEnumerable<CottageReservation> GetAllFutureByOwnerId(string ownerId, IEnumerable<Cottage> cottages);
        IEnumerable<CottageReservation> GetAllHistoryByOwnerId(string ownerId, IEnumerable<Cottage> cottages);
        IEnumerable<CottageReservation> GetAllByCottageId(string cottageId);
    }
}
