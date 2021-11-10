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
    public class BoatReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatReservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatReservation.ToListAsync());
        }

        // GET: BoatReservations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservation = await _context.BoatReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReservation == null)
            {
                return NotFound();
            }

            return View(boatReservation);
        }

        // GET: BoatReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatId,UserDetailsId,StartDate,EndDate,Price,PersonCount,Id,RowVersion")] BoatReservation boatReservation)
        {
            if (ModelState.IsValid)
            {
                boatReservation.Id = Guid.NewGuid();
                _context.Add(boatReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatReservation);
        }

        // GET: BoatReservations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservation = await _context.BoatReservation.FindAsync(id);
            if (boatReservation == null)
            {
                return NotFound();
            }
            return View(boatReservation);
        }

        // POST: BoatReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatId,UserDetailsId,StartDate,EndDate,Price,PersonCount,Id,RowVersion")] BoatReservation boatReservation)
        {
            if (id != boatReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatReservationExists(boatReservation.Id))
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
            return View(boatReservation);
        }

        // GET: BoatReservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservation = await _context.BoatReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReservation == null)
            {
                return NotFound();
            }

            return View(boatReservation);
        }

        // POST: BoatReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatReservation = await _context.BoatReservation.FindAsync(id);
            _context.BoatReservation.Remove(boatReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatReservationExists(Guid id)
        {
            return _context.BoatReservation.Any(e => e.Id == id);
        }
    }
}
