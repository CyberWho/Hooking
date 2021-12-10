using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Hooking.Models.DTO
{
    public class AdventureDTO
    {
        public string AdventureId { get; set; }
        public string RowVersion { get; set; }
        public string InstructorId { get; set; }
        [Display(Name="Instruktor")]
        public string InstructorName { get; set; }
        [Display(Name = "Naziv avanture")]
        public string Name { get; set; }
        [Display(Name = "Adresa")]
        public string Address { get; set; }
        [Display(Name = "Grad")]
        public string City { get; set; }
        [Display(Name = "Država")]
        public string Country { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Maksimalan broj osoba")]
        public string MaxPersonCount { get; set; }
        public string CancellationPolicyId { get; set; }
        [Display(Name = "Otkazivanje")]
        public string CancelationPolicyDescription { get; set; }
        [Display(Name = "Prosečna ocena")]
        public string AverageGrade { get; set; }

        public AdventureDTO(){}

        public AdventureDTO(Adventure adventure)
        {
            AdventureId = adventure.Id.ToString();
            RowVersion = adventure.RowVersion.ToString();
            InstructorId = adventure.InstructorId;
            Name = adventure.Name;
            Address = adventure.Address;
            City = adventure.City;
            Country = adventure.Country;
            Description = adventure.Description;
            MaxPersonCount = adventure.MaxPersonCount.ToString();
            CancellationPolicyId = adventure.CancellationPolicyId;
            AverageGrade = adventure.AverageGrade.ToString();
        }

        public void PopulateFieldsFromCancellationPolicy(CancelationPolicy policy)
        {
            CancelationPolicyDescription = $"Besplatno {policy.FreeUntil} dana, kazna: {policy.PenaltyPercentage}%";
        }

    }
}
