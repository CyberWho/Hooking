using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageRoomsService
    {
        CottageRoom GetById(Guid id);
        bool Create(CottageRoom cottageRoom);
        bool DeleteById(Guid id);
        bool AddToCottage(CottagesRooms cottagesRooms);
        CottageRoom Update(Guid id,CottageRoom cottageRoom);
        IEnumerable<CottageRoom> GetAllByCottageId(Guid id);
    }
}
