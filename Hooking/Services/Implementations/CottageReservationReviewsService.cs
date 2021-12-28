using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class CottageReservationReviewsService : ICottageReservationReviewsService
    {
        private readonly ApplicationDbContext _context;
        public CottageReservationReviewsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Create(CottageReservationReview cottageReservationReview)
        {
            _context.CottageReservationReview.Add(cottageReservationReview);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            var cottageReservationReview = GetById(id);
            _context.CottageReservationReview.Remove(cottageReservationReview);
            _context.SaveChanges();
            return true;
        }

        public CottageReservationReview GetById(Guid id)
        {
            return _context.CottageReservationReview.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
