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
    public class Tests
    {
        public CottageNotAvailablePeriodsController cottageNotAvailablePeriodsController; 
        private ApplicationDbContext _context;
        public CottageNotAvailablePeriod cottageNotAvailablePeriod;
        public Cottage cottage;
        [SetUp]
        public void Setup()
        {
            var dbOption = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Server=tcp:hooking.database.windows.net,1433;Initial Catalog=HookingDB;Persist Security Info=False;User ID=pedja;Password=Omorika7212.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;").Options;
            _context = new ApplicationDbContext(dbOption);
            cottageNotAvailablePeriodsController = new CottageNotAvailablePeriodsController(_context);
            cottage = _context.Cottage.Find(1);
            cottageNotAvailablePeriod = new CottageNotAvailablePeriod();
            cottageNotAvailablePeriod.Id = Guid.NewGuid();
            cottageNotAvailablePeriod.CottageId = cottage.Id.ToString();
            cottageNotAvailablePeriod.StartTime = DateTime.Now;
            cottageNotAvailablePeriod.EndTime = DateTime.Now.AddDays(2);
        }

        [Test]
        public async Task CreatingNewCottageNotAvailablePeriod()
        {

            cottageNotAvailablePeriodsController = new CottageNotAvailablePeriodsController(_context);
            await cottageNotAvailablePeriodsController.Create(cottage.Id,cottageNotAvailablePeriod);
            var nullPeriod = _context.CottageNotAvailablePeriod.Find(cottageNotAvailablePeriod.Id);
            Assert.IsNull(nullPeriod);
        }
    }
}