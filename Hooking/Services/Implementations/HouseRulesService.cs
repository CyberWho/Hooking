using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class HouseRulesService : IHouseRulesService
    {
        private readonly ApplicationDbContext _context;
        public HouseRulesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(Guid id,HouseRules houseRules)
        {
            houseRules.Id = Guid.NewGuid();
            _context.Add(houseRules);
            _context.SaveChanges();
            CottagesHouseRules cottagesHouseRules = new CottagesHouseRules();
            cottagesHouseRules.Id = Guid.NewGuid();
            cottagesHouseRules.CottageId = id.ToString();
            cottagesHouseRules.HouseRulesId = houseRules.Id.ToString();
            _context.Add(cottagesHouseRules);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            var houseRules = GetById(id);
            _context.HouseRules.Remove(houseRules);
            _context.SaveChanges();
            return true;
        }

        public HouseRules GetByCottageId(Guid cottageId)
        {
            var cId = cottageId.ToString();
            CottagesHouseRules cottagesHouseRules = _context.CottagesHouseRules.Where(m => m.CottageId == cId).FirstOrDefault<CottagesHouseRules>();
            Guid houseRulesId = Guid.Parse(cottagesHouseRules.HouseRulesId);
            return _context.HouseRules.Where(m => m.Id == houseRulesId).FirstOrDefault<HouseRules>();
        }

        public CottagesHouseRules GetByHouseRulesId(Guid houseRulesId)
        {
            var hId = houseRulesId.ToString();
            return _context.CottagesHouseRules.Where(m => m.HouseRulesId == hId).FirstOrDefault<CottagesHouseRules>();

        }

        public HouseRules GetById(Guid id)
        {
            return _context.HouseRules.Where(m => m.Id == id).FirstOrDefault();
        }

        public HouseRules Update(Guid id, HouseRules houseRules)
        {
            var houseRulesTmp = GetById(id);
            houseRulesTmp.PetFriendly = houseRules.PetFriendly;
            houseRulesTmp.NonSmoking = houseRules.NonSmoking;
            houseRulesTmp.CheckInTime = houseRules.CheckInTime;
            houseRulesTmp.CheckOutTime = houseRules.CheckOutTime;
            houseRulesTmp.AgeRestriction = houseRules.AgeRestriction;
            _context.Update(houseRulesTmp);
            _context.SaveChanges();
            return houseRulesTmp;
        }
    }
}
