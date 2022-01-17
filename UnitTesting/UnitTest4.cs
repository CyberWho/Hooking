using Hooking.Data;
using Hooking.Models;
using Hooking.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hooking.Models.DTO;
using Moq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Hooking.Services.Implementations;

namespace UnitTesting
{
    class UnitTest4
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        public AdventureRealisationsController adventureRealisationsController;
        [SetUp]
        public void SetUp()
        {
            var dbOption = new DbContextOptionsBuilder<ApplicationDbContext>()
                           .UseSqlServer("Server=tcp:hooking.database.windows.net,1433;Initial Catalog=HookingDB;Persist Security Info=False;User ID=pedja;Password=Omorika7212.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;").Options;
            _context = new ApplicationDbContext(dbOption);
            adventureRealisationsController = new AdventureRealisationsController(_context, _userManager);
            
        }
        [Test]
        public async Task CreateAdventureRealisation()
        {
            Adventure adventure = await _context.Adventure.FirstOrDefaultAsync();
            AdventureRealisation adventureRealisation = new AdventureRealisation()
            {
                AdventureId = adventure.Id.ToString(),
                Duration = 2,
                Id = Guid.NewGuid(),
                Price = 3000,
                StartDate = DateTime.Now
            };
            await adventureRealisationsController.Create(adventureRealisation);
            var adventureRealisationTemp = _context.AdventureRealisation.Find(adventureRealisation.Id);
            Assert.IsInstanceOf<AdventureRealisation>(adventureRealisationTemp);
        }

    }
}
