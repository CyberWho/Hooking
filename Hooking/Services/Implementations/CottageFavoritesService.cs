using Hooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;

namespace Hooking.Services.Implementations
{
    public class CottageFavoritesService : ICottageFavoritesService
    {
        private readonly ApplicationDbContext _context; 
        public CottageFavoritesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CottageFavorites cottageFavorites)
        {
            _context.CottageFavorites.Add(cottageFavorites);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<CottageFavorites> GetAllByCottageId(Guid cottageId)
        {
            var cId = cottageId.ToString();
            return _context.CottageFavorites.Where(m => m.CottageId == cId).ToList();
            
        }

        public CottageFavorites GetById(Guid id)
        {
            return _context.CottageFavorites.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
