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
    public class AdventureSpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureSpecialOffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureSpecialOffers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureSpecialOffer.ToListAsync());
        }

        // GET: AdventureSpecialOffers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }

            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureSpecialOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdventureRealisationId,UserDetailsId,Description,Id,RowVersion")] AdventureSpecialOffer adventureSpecialOffer)
        {
            if (ModelState.IsValid)
            {
                adventureSpecialOffer.Id = Guid.NewGuid();
                _context.Add(adventureSpecialOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer.FindAsync(id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }
            return View(adventureSpecialOffer);
        }

        // POST: AdventureSpecialOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdventureRealisationId,UserDetailsId,Description,Id,RowVersion")] AdventureSpecialOffer adventureSpecialOffer)
        {
            if (id != adventureSpecialOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureSpecialOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureSpecialOfferExists(adventureSpecialOffer.Id))
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
            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }

            return View(adventureSpecialOffer);
        }

        // POST: AdventureSpecialOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureSpecialOffer = await _context.AdventureSpecialOffer.FindAsync(id);
            _context.AdventureSpecialOffer.Remove(adventureSpecialOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureSpecialOfferExists(Guid id)
        {
            return _context.AdventureSpecialOffer.Any(e => e.Id == id);
        }
    }
}
