using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Services;

namespace Hooking.Controllers
{
    public class CottageRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICottageRoomsService _cottageRoomsService;
        private readonly ICottageService _cottageService;
        public Cottage cottage;
        public CottageRoomsController(ApplicationDbContext context,
                                        ICottageRoomsService cottageRoomsService,
                                        ICottageService cottageService)
        {
            _context = context;
            _cottageRoomsService = cottageRoomsService;
            _cottageService = cottageService;
        }

        // GET: CottageRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageRoom.ToListAsync());
        }

        // GET: CottageRooms/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = _cottageRoomsService.GetById(id);
            if (cottageRoom == null)
            {
                return NotFound();
            }

            return View(cottageRoom);
        }

        // GET: CottageRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CottageRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CottageRooms/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("BedCount,AirCondition,TV,Balcony,Id,RowVersion")] CottageRoom cottageRoom)
        {
            if (ModelState.IsValid)
            {
                cottageRoom.Id = Guid.NewGuid();
                _cottageRoomsService.Create(cottageRoom);
                Cottage cottage = _cottageService.GetCottageById(id);
                CottagesRooms cottagesRooms = new CottagesRooms();
                cottagesRooms.Id = Guid.NewGuid();
                cottagesRooms.CottageRoomId = cottageRoom.Id.ToString();
                cottagesRooms.CottageId = cottage.Id.ToString();
                _cottageRoomsService.AddToCottage(cottagesRooms);
                return RedirectToAction("AddRooms", "CottageRooms", new { id = cottage.Id});
            }
            return View(cottageRoom);
        }
        [HttpGet("/CottageRooms/AddRooms/{id}")]
        public async Task<IActionResult> AddRooms(Guid id)
        {
            cottage = _cottageService.GetCottageById(id);
            ViewData["Cottage"] = cottage;
            return View();
        }
        // GET: CottageRooms/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = _cottageRoomsService.GetById(id);
            if (cottageRoom == null)
            {
                return NotFound();
            }
            return View(cottageRoom);
        }

        // POST: CottageRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/CottageRooms/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BedCount,AirCondition,TV,Balcony,Id,RowVersion")] CottageRoom cottageRoom)
        {
            if (id != cottageRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _cottageRoomsService.Update(id, cottageRoom);
                    var cottageRoomId = cottageRoom.Id.ToString();
                    CottagesRooms cottagesRooms = _context.CottagesRooms.Where(m => m.CottageRoomId == cottageRoomId).FirstOrDefault<CottagesRooms>();
                    Guid cottageId = Guid.Parse(cottagesRooms.CottageId);
                    var cottage = _cottageService.GetCottageById(cottageId);
                    return RedirectToAction("MyCottage", "Cottages", new { id = cottage.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageRoomExists(cottageRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(cottageRoom);
        }

        // GET: CottageRooms/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = _cottageRoomsService.GetById(id);
            if (cottageRoom == null)
            {
                return NotFound();
            }

            return View(cottageRoom);
        }

        // POST: CottageRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            _cottageRoomsService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CottageRoomExists(Guid id)
        {
            return _context.CottageRoom.Any(e => e.Id == id);
        }
    }
}
