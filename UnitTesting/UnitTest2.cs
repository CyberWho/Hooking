using NUnit.Framework;
using Hooking.Controllers;
using Microsoft.AspNetCore.Identity;
using Hooking.Data;
using Hooking.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace UnitTesting
{
    class UnitTest2
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public BoatsController boatsController;
        [SetUp]
        public void SetUp()
        {
            var dbOption = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Server=tcp:hooking.database.windows.net,1433;Initial Catalog=HookingDB;Persist Security Info=False;User ID=pedja;Password=Omorika7212.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;").Options;
            _context = new ApplicationDbContext(dbOption);
            boatsController = new BoatsController(_context, _userManager, _roleManager);
            
        }
        #region Unit Test
        [Test]
        public async Task EditBoat()
        {
            Boat boat = await _context.Boat.FirstOrDefaultAsync();
            double regPrice = boat.RegularPrice;
            boat.RegularPrice = boat.RegularPrice + 10000;
            var actionResult = boatsController.Edit(boat.Id, boat);
            Assert.AreNotEqual(regPrice, boat.RegularPrice);
        }
        #endregion
    }
}
