using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageFavoritesService
    {
        IEnumerable<CottageFavorites> GetAllByCottageId(Guid cottageId);
        bool Create(CottageFavorites cottageFavorites);
        CottageFavorites GetById(Guid id);
    }
}
