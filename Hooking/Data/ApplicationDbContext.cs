using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Hooking.Models;

namespace Hooking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Hooking.Models.CancelationPolicy> CancelationPolicy { get; set; }
        public DbSet<Hooking.Models.Cottage> Cottage { get; set; }
        public DbSet<Hooking.Models.CottageOwnerReview> CottageOwnerReview { get; set; }
        public DbSet<Hooking.Models.CottageReservation> CottageReservation { get; set; }
        public DbSet<Hooking.Models.CottageReservationReview> CottageReservationReview { get; set; }
        public DbSet<Hooking.Models.CottageReview> CottageReview { get; set; }
        public DbSet<Hooking.Models.CottageRoom> CottageRoom { get; set; }
        public DbSet<Hooking.Models.CottageSpecialOffer> CottageSpecialOffer { get; set; }
        public DbSet<Hooking.Models.CottagesRooms> CottagesRooms { get; set; }
        public DbSet<Hooking.Models.Facilities> Facilities { get; set; }
        public DbSet<Hooking.Models.HouseRules> HouseRules { get; set; }
        public DbSet<Hooking.Models.CottagesFacilities> CottagesFacilities { get; set; }
        public DbSet<Hooking.Models.CottagesHouseRules> CottagesHouseRules { get; set; }
    }
}
