using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hooking.Models
{
    [Table("tbUser")]
    public class User
    {
        [ForeignKey("IdentityUser")]
        public Guid IdentityUserId { get; }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData] 
        public string Address { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string Country { get; set; }

        public User(IdentityUser identityUser, string firstName, string lastName, string address, string city, string country)
        {
            IdentityUserId = Guid.Parse(identityUser.Id);
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            Country = country;
        }
    }
}
