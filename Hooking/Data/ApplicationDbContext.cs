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
        public DbSet<Hooking.Models.UserDetails> UserDetails { get; set; }
        public DbSet<Hooking.Models.BoatOwner> BoatOwner { get; set; }
        public DbSet<Hooking.Models.CottageOwner> CottageOwner { get; set; }
        public DbSet<Hooking.Models.Instructor> Instructor { get; set; }
    }
}
