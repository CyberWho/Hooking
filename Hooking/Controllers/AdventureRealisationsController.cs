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
    public class AdventureRealisationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureRealisationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureRealisations
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureRealisation.ToListAsync());
        }

        // GET: AdventureRealisations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureRealisation = await _context.AdventureRealisation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureRealisation == null)
            {
                return NotFound();
            }

            return View(adventureRealisation);
        }

        // GET: AdventureRealisations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureRealisations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdventureId,Duration,Price,StartDate,Id,RowVersion")] AdventureRealisation adventureRealisation)
        {
            if (ModelState.IsValid)
            {
                adventureRealisation.Id = Guid.NewGuid();
                _context.Add(adventureRealisation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureRealisation);
        }

        // GET: AdventureRealisations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureRealisation = await _context.AdventureRealisation.FindAsync(id);
            if (adventureRealisation == null)
            {
                return NotFound();
            }
            return View(adventureRealisation);
        }

        // POST: AdventureRealisations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdventureId,Duration,Price,StartDate,Id,RowVersion")] AdventureRealisation adventureRealisation)
        {
            if (id != adventureRealisation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureRealisation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureRealisationExists(adventureRealisation.Id))
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
            return View(adventureRealisation);
        }

        // GET: AdventureRealisations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureRealisation = await _context.AdventureRealisation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureRealisation == null)
            {
                return NotFound();
            }

            return View(adventureRealisation);
        }

        // POST: AdventureRealisations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureRealisation = await _context.AdventureRealisation.FindAsync(id);
            _context.AdventureRealisation.Remove(adventureRealisation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureRealisationExists(Guid id)
        {
            return _context.AdventureRealisation.Any(e => e.Id == id);
        }
    }
}
