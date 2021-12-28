using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface ICottageReservationReviewsService
    {
        CottageReservationReview GetById(Guid id);
        bool Create(CottageReservationReview cottageReservationReview);
        bool DeleteById(Guid id);
    }
}
