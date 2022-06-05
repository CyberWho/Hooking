using Hooking.Controllers;
using Hooking.Data;
using Hooking.Data.Migrations;
using Hooking.Models;
using Hooking.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    class UnitTest7
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        private IAdventureService _adventureService;
        private IEmailSender _emailSender;

        public AdventureReservationsController adventureReservationsController;
        public AdventureRealisation adventureRealisation;
        [SetUp]
        public void SetUp()
        {
            var dbOption = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Data Source=DESKTOP-CJ8VDR7;Initial Catalog=HookingDB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;").Options;
            _context = new ApplicationDbContext(dbOption);

            adventureReservationsController = new AdventureReservationsController(_context, _adventureService, _userManager, _emailSender);
            adventureRealisation = new AdventureRealisation();
            adventureRealisation.Id = new Guid();
            adventureRealisation.Price = 300;
            adventureRealisation.Duration = 3;
            adventureRealisation.AdventureId = "f3b33200-3441-4fdf-b826-acd44b6d2b95";
            adventureRealisation.StartDate = DateTime.Parse("2022-06-15 15:40:45.0000000");
            Console.WriteLine("datum je " + adventureRealisation.StartDate.ToString());



        }
        #region Unit Test
        [Test]
        public void AdventureRealisationExists()
        {
            string exists = adventureReservationsController.adventureRealisationExists(adventureRealisation);
            Assert.IsNotEmpty(exists);
        }
        #endregion
    }
}
