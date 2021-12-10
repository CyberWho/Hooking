using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;
using Hooking.Models.DTO;

namespace Hooking.Services
{
    public interface IAdventureService
    {
        IEnumerable<AdventureReservationDTO> GetAdventureReservations(Guid instructorId);
        IEnumerable<AdventureDTO> GetInstructorAdventures(string userId);
        bool AdventureEditable(Guid adventureId);
    }
}
