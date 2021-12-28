using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Models;

namespace Hooking.Services
{
    public interface IHouseRulesService
    {
        HouseRules GetById(Guid id);
        bool Create(Guid id, HouseRules houseRules);
        bool DeleteById(Guid id);
        HouseRules GetByCottageId(Guid cottageId);
        HouseRules Update(Guid id, HouseRules houseRules);
        CottagesHouseRules GetByHouseRulesId(Guid houseRulesId);
    }
}
