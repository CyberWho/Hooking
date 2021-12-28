using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class CottageSpecialOffersService : ICottageSpecialOffersService
    {
        private readonly ApplicationDbContext _context;
        public CottageSpecialOffersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CottageSpecialOffer cottageSpecialOffer)
        {
            _context.CottageSpecialOffer.Add(cottageSpecialOffer);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            var cottageSpecialOffer = GetById(id);
            _context.CottageSpecialOffer.Remove(cottageSpecialOffer);
            _context.SaveChanges();
            return true;
        }

        public CottageSpecialOffer GetById(Guid id)
        {
            return _context.CottageSpecialOffer.Where(m => m.Id == id).FirstOrDefault();
        }

        public CottageSpecialOffer Update(Guid id, CottageSpecialOffer cottageSpecialOffer)
        {
            var cottageSpecialOfferTmp = GetById(id);
            cottageSpecialOfferTmp.Id = id;
            cottageSpecialOfferTmp.CottageId = cottageSpecialOffer.CottageId;
            cottageSpecialOfferTmp.StartDate = cottageSpecialOffer.StartDate;
            cottageSpecialOfferTmp.EndDate = cottageSpecialOffer.EndDate;
            cottageSpecialOfferTmp.Price = cottageSpecialOffer.Price;
            cottageSpecialOfferTmp.MaxPersonCount = cottageSpecialOffer.MaxPersonCount;
            cottageSpecialOfferTmp.Description = cottageSpecialOffer.Description;
            cottageSpecialOfferTmp.IsReserved = cottageSpecialOffer.IsReserved;
            _context.Update(cottageSpecialOfferTmp);
             _context.SaveChanges();
            return cottageSpecialOfferTmp;
        }
    }
}
