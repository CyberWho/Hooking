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
    public class BoatNotAvailablePeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatNotAvailablePeriodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatNotAvailablePeriods
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatNotAvailablePeriod.ToListAsync());
        }

        // GET: BoatNotAvailablePeriods/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatNotAvailablePeriod = await _context.BoatNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(boatNotAvailablePeriod);
        }

        // GET: BoatNotAvailablePeriods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatNotAvailablePeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatId,StartTime,EndTime,Id,RowVersion")] BoatNotAvailablePeriod boatNotAvailablePeriod)
        {
            if (ModelState.IsValid)
            {
                boatNotAvailablePeriod.Id = Guid.NewGuid();
                _context.Add(boatNotAvailablePeriod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatNotAvailablePeriod);
        }

        // GET: BoatNotAvailablePeriods/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatNotAvailablePeriod = await _context.BoatNotAvailablePeriod.FindAsync(id);
            if (boatNotAvailablePeriod == null)
            {
                return NotFound();
            }
            return View(boatNotAvailablePeriod);
        }

        // POST: BoatNotAvailablePeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatId,StartTime,EndTime,Id,RowVersion")] BoatNotAvailablePeriod boatNotAvailablePeriod)
        {
            if (id != boatNotAvailablePeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatNotAvailablePeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatNotAvailablePeriodExists(boatNotAvailablePeriod.Id))
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
            return View(boatNotAvailablePeriod);
        }

        // GET: BoatNotAvailablePeriods/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatNotAvailablePeriod = await _context.BoatNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(boatNotAvailablePeriod);
        }

        // POST: BoatNotAvailablePeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatNotAvailablePeriod = await _context.BoatNotAvailablePeriod.FindAsync(id);
            _context.BoatNotAvailablePeriod.Remove(boatNotAvailablePeriod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatNotAvailablePeriodExists(Guid id)
        {
            return _context.BoatNotAvailablePeriod.Any(e => e.Id == id);
        }
    }
}
