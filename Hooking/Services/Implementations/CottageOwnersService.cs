using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class CottageOwnersService : ICottageOwnersService
    {
        private readonly ApplicationDbContext _context;
        public CottageOwnersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CottageOwner cottageOwner)
        {
            _context.CottageOwner.Add(cottageOwner);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            var cottageOwner = GetById(id);
            _context.CottageOwner.Remove(cottageOwner);
            _context.SaveChanges();
            return true;
        }

        public CottageOwner GetById(Guid id)
        {
            return _context.CottageOwner.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
