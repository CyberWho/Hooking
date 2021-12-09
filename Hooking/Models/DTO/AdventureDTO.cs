using System;
using System.Collections.Generic;
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
        public string InstructorName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string MaxPersonCount { get; set; }
        public string CancellationPolicyId { get; set; }
        public string CancelationPolicyDescription { get; set; }
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

        public void PopulateFieldsFromInstructor(Instructor instructor, AdventureDTO dto)
        {
        }

    }
}
