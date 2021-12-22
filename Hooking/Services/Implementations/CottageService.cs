using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class CottageService : ICottageService
    {
        private readonly ApplicationDbContext _context;
        public CottageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Cottage Create(string ownerId,Cottage cottage)
        {
            cottage.Id = Guid.NewGuid();
            cottage.CottageOwnerId = ownerId;
            cottage.CancelationPolicyId = "0";
            cottage.AverageGrade = 0;
            cottage.GradeCount = 0;
            _context.Add(cottage);
            _context.SaveChanges();
            return cottage;
        }
        
        public bool DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(Guid id, Cottage editedCottage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cottage> GetAllByOwnerId(string ownerId)
        {
            return _context.Cottage.Where(m => m.CottageOwnerId == ownerId).ToList();
        }

        public Cottage GetCottageById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool UploadImage(Guid id, IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
