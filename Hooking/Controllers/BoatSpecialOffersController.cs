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
    public class BoatSpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatSpecialOffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatSpecialOffers
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatSpecialOffer.ToListAsync());
        }

        // GET: BoatSpecialOffers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatSpecialOffer = await _context.BoatSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatSpecialOffer == null)
            {
                return NotFound();
            }

            return View(boatSpecialOffer);
        }

        // GET: BoatSpecialOffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatSpecialOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatId,UserDetailsId,StartDate,EndDate,Price,MaxPersonCount,Description,Id,RowVersion")] BoatSpecialOffer boatSpecialOffer)
        {
            if (ModelState.IsValid)
            {
                boatSpecialOffer.Id = Guid.NewGuid();
                _context.Add(boatSpecialOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatSpecialOffer);
        }

        // GET: BoatSpecialOffers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatSpecialOffer = await _context.BoatSpecialOffer.FindAsync(id);
            if (boatSpecialOffer == null)
            {
                return NotFound();
            }
            return View(boatSpecialOffer);
        }

        // POST: BoatSpecialOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatId,UserDetailsId,StartDate,EndDate,Price,MaxPersonCount,Description,Id,RowVersion")] BoatSpecialOffer boatSpecialOffer)
        {
            if (id != boatSpecialOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatSpecialOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatSpecialOfferExists(boatSpecialOffer.Id))
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
            return View(boatSpecialOffer);
        }

        // GET: BoatSpecialOffers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatSpecialOffer = await _context.BoatSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatSpecialOffer == null)
            {
                return NotFound();
            }

            return View(boatSpecialOffer);
        }

        // POST: BoatSpecialOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatSpecialOffer = await _context.BoatSpecialOffer.FindAsync(id);
            _context.BoatSpecialOffer.Remove(boatSpecialOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatSpecialOfferExists(Guid id)
        {
            return _context.BoatSpecialOffer.Any(e => e.Id == id);
        }
    }
}
