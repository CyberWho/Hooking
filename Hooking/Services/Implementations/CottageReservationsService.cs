using Hooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;

namespace Hooking.Services.Implementations
{
    public class CottageReservationsService : ICottageReservationsService
    {
        public ApplicationDbContext _context;
        public CottageReservationsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CottageReservation> GetAllByCottageId(string cottageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CottageReservation> GetAllFutureByOwnerId(string ownerId, IEnumerable<Cottage> cottages)
        {
            List<CottageReservation> cottageReservations = new List<CottageReservation>();
            
            foreach (var cottage in cottages)
            {
                var cottageId = cottage.Id.ToString();
                List<CottageReservation> myCottageReservations = new List<CottageReservation>();
                myCottageReservations = _context.CottageReservation.Where(m => m.CottageId == cottageId).ToList();
                if (myCottageReservations.Count != 0)
                {
                    foreach (CottageReservation cottageReservation in myCottageReservations)
                    {
                        if (cottageReservation.StartDate >= DateTime.Now)
                        {
                            cottageReservations.Add(cottageReservation);
                            
                        }
                    }
                }
                
            }
            return cottageReservations;
        }

        public IEnumerable<CottageReservation> GetAllHistoryByOwnerId(string ownerId, IEnumerable<Cottage> cottages)
        {
            List<Cottage> myCottages = _context.Cottage.Where(m => m.CottageOwnerId == ownerId).ToList();
            List<CottageReservation> cottageReservations = new List<CottageReservation>();
            
            foreach (var cottage in myCottages)
            {
                var cottageId = cottage.Id.ToString();
                List<CottageReservation> myCottageReservations = new List<CottageReservation>();
                myCottageReservations = _context.CottageReservation.Where(m => m.CottageId == cottageId).ToList();
                if (myCottageReservations.Count != 0)
                {
                    foreach (CottageReservation cottageReservation in myCottageReservations)
                    {
                        if (cottageReservation.StartDate <= DateTime.Now)
                        {
                            cottageReservations.Add(cottageReservation);
                            
                        }
                    }
                }

            }
            return cottageReservations;
        }
    }
}
