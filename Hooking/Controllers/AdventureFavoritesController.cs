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
    public class AdventureFavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureFavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureFavorites
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureFavorites.ToListAsync());
        }

        // GET: AdventureFavorites/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureFavorites = await _context.AdventureFavorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureFavorites == null)
            {
                return NotFound();
            }

            return View(adventureFavorites);
        }

        // GET: AdventureFavorites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureFavorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserDetailsId,AdventureId,Id,RowVersion")] AdventureFavorites adventureFavorites)
        {
            if (ModelState.IsValid)
            {
                adventureFavorites.Id = Guid.NewGuid();
                _context.Add(adventureFavorites);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureFavorites);
        }

        // GET: AdventureFavorites/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureFavorites = await _context.AdventureFavorites.FindAsync(id);
            if (adventureFavorites == null)
            {
                return NotFound();
            }
            return View(adventureFavorites);
        }

        // POST: AdventureFavorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserDetailsId,AdventureId,Id,RowVersion")] AdventureFavorites adventureFavorites)
        {
            if (id != adventureFavorites.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureFavorites);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureFavoritesExists(adventureFavorites.Id))
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
            return View(adventureFavorites);
        }

        // GET: AdventureFavorites/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureFavorites = await _context.AdventureFavorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureFavorites == null)
            {
                return NotFound();
            }

            return View(adventureFavorites);
        }

        // POST: AdventureFavorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureFavorites = await _context.AdventureFavorites.FindAsync(id);
            _context.AdventureFavorites.Remove(adventureFavorites);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureFavoritesExists(Guid id)
        {
            return _context.AdventureFavorites.Any(e => e.Id == id);
        }
    }
}
