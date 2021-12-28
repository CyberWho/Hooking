using Hooking.Models;
using Hooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hooking.Services.Implementations
{
    public class CottageRoomsService : ICottageRoomsService
    {
        private readonly ApplicationDbContext _context;
        public CottageRoomsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddToCottage(CottagesRooms cottagesRooms)
        {

            _context.CottagesRooms.Add(cottagesRooms);
            _context.SaveChanges();
            return true;
        }

        public bool Create(CottageRoom cottageRoom)
        {
            _context.CottageRoom.Add(cottageRoom);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteById(Guid id)
        {
            var cottageRoom = GetById(id);
            _context.CottageRoom.Remove(cottageRoom);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<CottageRoom> GetAllByCottageId(Guid id)
        {
            var cottageId = id.ToString();
            List<CottageRoom> cottageRooms = new List<CottageRoom>();
            List<CottagesRooms> cottagesRooms = _context.CottagesRooms.Where(m => m.CottageId == cottageId).ToList<CottagesRooms>();
            foreach (var cottagesRoom in cottagesRooms)
            {
                Guid cottageRoomId = Guid.Parse(cottagesRoom.CottageRoomId);
                var cottageRoom = _context.CottageRoom.Where(m => m.Id == cottageRoomId).FirstOrDefault<CottageRoom>();
                cottageRooms.Add(cottageRoom);
            }
            return cottageRooms;
        }

        public CottageRoom GetById(Guid id)
        {
            return _context.CottageRoom.Where(m => m.Id == id).FirstOrDefault();
        }

        public CottageRoom Update(Guid id,CottageRoom cottageRoom)
        {
            var cottageRoomTmp = GetById(id);
            cottageRoomTmp.BedCount = cottageRoom.BedCount;
            cottageRoomTmp.AirCondition = cottageRoom.AirCondition;
            cottageRoomTmp.TV = cottageRoom.TV;
            cottageRoomTmp.Balcony = cottageRoom.Balcony;
            _context.Update(cottageRoomTmp);
            _context.SaveChanges();
            return cottageRoomTmp;
        }
    }
}
