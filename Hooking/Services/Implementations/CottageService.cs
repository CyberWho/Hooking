using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;


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
            Cottage cottage = _context.Cottage.Where(m => m.Id == id).FirstOrDefault();
            _context.Cottage.Remove(cottage);
            _context.SaveChanges();
            return true;
        }

        public bool Edit(Guid id, Cottage editedCottage)
        {
            Cottage cottageTemp =  _context.Cottage.Find(id);
            cottageTemp.Name = editedCottage.Name;
            cottageTemp.Description = editedCottage.Description;
            cottageTemp.RegularPrice = editedCottage.RegularPrice;
            cottageTemp.WeekendPrice = editedCottage.WeekendPrice;
            _context.Update(cottageTemp);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Cottage> GetAllByOwnerId(string ownerId)
        {
            return _context.Cottage.Where(m => m.CottageOwnerId == ownerId).ToList();
        }

        public Cottage GetCottageById(Guid id)
        {
            return _context.Cottage.Where(m => m.Id == id).FirstOrDefault();
        }

        public bool UploadImage(Guid id, IFormFile file)
        { 
            return true;
        }

    }
}
