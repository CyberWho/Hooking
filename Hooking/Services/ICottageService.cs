using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;
using Microsoft.AspNetCore.Http;

namespace Hooking.Services
{
    public interface ICottageService
    {
        Cottage GetCottageById(Guid id);
        Cottage Create(string ownerId, Cottage cottage);
        bool Edit(Guid id,Cottage editedCottage);
        bool DeleteById(Guid id);
        bool UploadImage(Guid id, IFormFile file);
        IEnumerable<Cottage> GetAllByOwnerId(string ownerId);
        
    }
}
