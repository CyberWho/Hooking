using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;
using Hooking.Models;
using Hooking.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Hooking.Services.Implementations
{
    public class AdventureService : IAdventureService
    {
        private readonly ApplicationDbContext _context;

        public AdventureService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AdventureReservationDTO> GetAdventureReservations(Guid instructorId)
        {

            var allAdventures = _context.Adventure.ToList();
            var allAdventureRealisations = _context.AdventureRealisation.ToList();
            var allAdventureReservations = _context.AdventureReservation.ToList();

            List<AdventureReservationDTO> retVal = new List<AdventureReservationDTO>();

            foreach (Adventure adventure in allAdventures)
            {
                foreach (AdventureRealisation realisation in allAdventureRealisations)
                {
                    foreach(AdventureReservation reservation in allAdventureReservations)
                    {
                        if (reservation.AdventureRealisationId == realisation.Id.ToString() &&
                            realisation.AdventureId == adventure.Id.ToString())
                        {
                            AdventureReservationDTO dto = new AdventureReservationDTO(adventure, realisation, reservation);
                            UserDetails userDetails = _context.UserDetails.Find(Guid.Parse(dto.UserDetailsId));
                            if (userDetails == null)
                            {
                                continue;
                            }
                            dto.UserDetailsFirstName = userDetails.FirstName;
                            dto.UserDetailsLastName = userDetails.LastName;
                            retVal.Add(dto);
                        }
                    }
                }
            }


            return retVal;
        }

        public IEnumerable<AdventureDTO> GetInstructorAdventures(string userId)
        {
            Guid userDetailsId = GetUserDetailsIdFromUserId(userId);

            Guid instructorId = GetInstructorIdFromUserDetailsId(userDetailsId);

            IEnumerable<Adventure> adventures = _context.Adventure.Where(a => a.InstructorId == instructorId.ToString()).ToList();
            List<AdventureDTO> dtos = new List<AdventureDTO>();

            foreach (Adventure adventure in adventures)
            {
                AdventureDTO tempDto = new AdventureDTO(adventure);
                
                CancelationPolicy policy = _context.CancelationPolicy.Find(Guid.Parse(adventure.CancellationPolicyId));
                tempDto.PopulateFieldsFromCancellationPolicy(policy);

                tempDto.InstructorName = GetInstructorNameFromId(instructorId);
                dtos.Add(tempDto);
            }
            
            return dtos;
        }

        private string GetInstructorNameFromId(Guid instructorId)
        {
            Instructor instructor = _context.Instructor.Find(instructorId);
            UserDetails userDetails = _context.UserDetails.Find(Guid.Parse(instructor.UserDetailsId));

            return $"{userDetails.FirstName} {userDetails.LastName}";
        }

        private Guid GetUserDetailsIdFromUserId(string userId)
        {
            return _context.UserDetails.Where(user => user.IdentityUserId == userId)
                .Select(user => user.Id)
                .FirstOrDefault();
        }

        private Guid GetInstructorIdFromUserDetailsId(Guid userDetailsId)
        {
            return _context.Instructor.Where(inst => inst.UserDetailsId == userDetailsId.ToString())
                .Select(inst => inst.Id)
                .FirstOrDefault();
        }
    }
}
