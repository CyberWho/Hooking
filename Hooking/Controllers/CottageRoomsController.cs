using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;

namespace Hooking.Controllers
{
    public class CottageRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CottageRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CottageRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageRoom.ToListAsync());
        }

        // GET: CottageRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = await _context.CottageRoom
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(cottageRoom);
                await _context.SaveChangesAsync();
                Cottage cottage = _context.Cottage.Where(m => m.Id == id).FirstOrDefault<Cottage>();
                CottagesRooms cottagesRooms = new CottagesRooms();
                cottagesRooms.Id = Guid.NewGuid();
                cottagesRooms.CottageRoomId = cottageRoom.Id.ToString();
                cottagesRooms.CottageId = cottage.Id.ToString();
                _context.Add(cottagesRooms);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return View(cottageRoom);
        }

        // GET: CottageRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = await _context.CottageRoom.FindAsync(id);
            if (cottageRoom == null)
            {
                return NotFound();
            }
            return View(cottageRoom);
        }

        // POST: CottageRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                    _context.Update(cottageRoom);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(cottageRoom);
        }

        // GET: CottageRooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageRoom = await _context.CottageRoom
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var cottageRoom = await _context.CottageRoom.FindAsync(id);
            _context.CottageRoom.Remove(cottageRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageRoomExists(Guid id)
        {
            return _context.CottageRoom.Any(e => e.Id == id);
        }
    }
}
