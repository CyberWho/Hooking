using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;
using Hooking.Models;
using Hooking.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                tempDto.InstructorBiography = _context.Instructor.Find(instructorId).Biography;
                dtos.Add(tempDto);
            }
            
            return dtos;
        }

        public bool AdventureEditable(Guid adventureId)
        {
            List<AdventureRealisation> realisations = _context.AdventureRealisation.Where(a => a.AdventureId == adventureId.ToString() && DateTime.Now >= a.StartDate).ToList();
            if (realisations.Count == 0)
            {
                return true;
            }

            foreach (AdventureRealisation realisation in realisations)
            {
                AdventureReservation reservation = _context.AdventureReservation.FirstOrDefault(r => r.AdventureRealisationId == realisation.Id.ToString());

                if (reservation == null)
                {
                    return true;
                }
            }
            
            return false;
        }

        public AdventureDTO GetAdventureDto(Guid adventureId)
        {
            Adventure adventure = _context.Adventure.Find(adventureId);
            AdventureDTO dto = new AdventureDTO(adventure);
            CancelationPolicy policy = _context.CancelationPolicy.Find(Guid.Parse(adventure.CancellationPolicyId));
            dto.PopulateFieldsFromCancellationPolicy(policy);

            dto.InstructorName = GetInstructorNameFromId(Guid.Parse(adventure.InstructorId));
            dto.InstructorBiography = _context.Instructor.Find(Guid.Parse(adventure.InstructorId)).Biography;

            var adventureEquipment =
                _context.AdventureFishingEquipment.FirstOrDefault(e => e.AdventureId == adventureId.ToString());

            string rulesId = _context.AdventuresAdventureRules
                .FirstOrDefault(r => r.AdventureId == adventureId.ToString()).AdventureRulesId;

            AdventureRules rules = _context.AdventureRules.Find(Guid.Parse(rulesId));

            if (adventureEquipment == null)
            {
                dto.PopulateFieldsFromRulesWithoutFishing(rules);
                return dto;
            }
            FishingEquipment equipment = _context.FishingEquipment.FirstOrDefault(e => e.Id == Guid.Parse(adventureEquipment.FishingEquipmentId));
            dto.PopulateFieldsFromFishingEquipment(equipment);
            dto.PopulateFieldsFromRulesWithFishing(rules);
            return dto;
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
