using Hooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;

namespace Hooking.Services.Implementations
{
    public class CottageNotAvailablePeriodsService : ICottageNotAvailablePeriodsService
    {
        private readonly ApplicationDbContext _context;
        public CottageNotAvailablePeriodsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CottageNotAvailablePeriod cottageNotAvailablePeriod)
        {
            _context.Add(cottageNotAvailablePeriod);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<CottageNotAvailablePeriod> GetAllByCottageId(Guid cottageId)
        {
            List<CottageNotAvailablePeriod> cottageNotAvailablePeriods = new List<CottageNotAvailablePeriod>();
            var cId = cottageId.ToString();
            cottageNotAvailablePeriods = _context.CottageNotAvailablePeriod.Where(m => m.CottageId == cId).ToList();
            return cottageNotAvailablePeriods;
        }

        public CottageNotAvailablePeriod GetById(Guid id)
        {
            return _context.CottageNotAvailablePeriod.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
