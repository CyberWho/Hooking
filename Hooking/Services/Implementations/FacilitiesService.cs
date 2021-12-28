using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class FacilitiesService : IFacilitiesService
    {
        private readonly ApplicationDbContext _context;
        public FacilitiesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddToCottage(CottagesFacilities cottagesFacilities)
        {

            _context.CottagesFacilities.Add(cottagesFacilities);
            _context.SaveChanges();
            return true;
        }

        public bool Create(Guid id, Facilities facilities)
        {
            _context.Facilities.Add(facilities);
            _context.SaveChanges();
            CottagesFacilities cottagesFacilities = new CottagesFacilities();
            cottagesFacilities.Id = Guid.NewGuid();
            cottagesFacilities.CottageId = id.ToString();
            cottagesFacilities.FacilitiesId = facilities.Id.ToString();
            _context.Add(cottagesFacilities);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Facilities GetByCottageId(Guid cottageId)
        {
            throw new NotImplementedException();
        }

        public CottagesFacilities GetByFacilitiesId(Guid id)
        {

            var facilitiesId = id.ToString();
            return _context.CottagesFacilities.Where(m => m.FacilitiesId == facilitiesId).FirstOrDefault();
        }

        public Facilities GetById(Guid id)
        {
            return _context.Facilities.Where(m => m.Id == id).FirstOrDefault();
        }

        public Facilities Update(Guid id, Facilities facilities)
        {
            var facilitiesTmp = _context.Facilities.Find(id);
            facilitiesTmp.Parking = facilities.Parking;
            facilitiesTmp.Wifi = facilities.Wifi;
            facilitiesTmp.Heating = facilities.Heating;
            facilitiesTmp.BarbecueFacilities = facilities.BarbecueFacilities;
            facilitiesTmp.OnlineCheckin = facilities.OnlineCheckin;
            facilitiesTmp.Jacuzzi = facilities.Jacuzzi;
            facilitiesTmp.SeaView = facilities.SeaView;
            facilitiesTmp.MountainView = facilities.MountainView;
            facilitiesTmp.Kitchen = facilities.Kitchen;
            facilitiesTmp.WashingMachine = facilities.WashingMachine;
            facilitiesTmp.AirportShuttle = facilities.AirportShuttle;
            facilitiesTmp.Garden = facilities.Garden;
            facilitiesTmp.IndoorPool = facilities.IndoorPool;
            facilities.OutdoorPool = facilities.OutdoorPool;
            facilitiesTmp.StockedBar = facilities.StockedBar;
            _context.Update(facilitiesTmp);
             _context.SaveChanges();
            return facilitiesTmp;
        }
    }
}
