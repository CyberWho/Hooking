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
    public class AdventureReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureReservation.ToListAsync());
        }

        // GET: AdventureReservations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReservation = await _context.AdventureReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureReservation == null)
            {
                return NotFound();
            }

            return View(adventureReservation);
        }

        // GET: AdventureReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdventureRealisationId,UserDetailsId,Id,RowVersion")] AdventureReservation adventureReservation)
        {
            if (ModelState.IsValid)
            {
                adventureReservation.Id = Guid.NewGuid();
                _context.Add(adventureReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureReservation);
        }

        // GET: AdventureReservations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReservation = await _context.AdventureReservation.FindAsync(id);
            if (adventureReservation == null)
            {
                return NotFound();
            }
            return View(adventureReservation);
        }

        // POST: AdventureReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdventureRealisationId,UserDetailsId,Id,RowVersion")] AdventureReservation adventureReservation)
        {
            if (id != adventureReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureReservationExists(adventureReservation.Id))
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
            return View(adventureReservation);
        }

        // GET: AdventureReservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReservation = await _context.AdventureReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureReservation == null)
            {
                return NotFound();
            }

            return View(adventureReservation);
        }

        // POST: AdventureReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureReservation = await _context.AdventureReservation.FindAsync(id);
            _context.AdventureReservation.Remove(adventureReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureReservationExists(Guid id)
        {
            return _context.AdventureReservation.Any(e => e.Id == id);
        }
    }
}
