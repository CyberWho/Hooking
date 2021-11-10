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
    public class BoatReservationReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatReservationReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatReservationReviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatReservationReview.ToListAsync());
        }

        // GET: BoatReservationReviews/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservationReview = await _context.BoatReservationReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReservationReview == null)
            {
                return NotFound();
            }

            return View(boatReservationReview);
        }

        // GET: BoatReservationReviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatReservationReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatReservationId,Review,DidntShow,ReceivedPenalty,Id,RowVersion")] BoatReservationReview boatReservationReview)
        {
            if (ModelState.IsValid)
            {
                boatReservationReview.Id = Guid.NewGuid();
                _context.Add(boatReservationReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatReservationReview);
        }

        // GET: BoatReservationReviews/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservationReview = await _context.BoatReservationReview.FindAsync(id);
            if (boatReservationReview == null)
            {
                return NotFound();
            }
            return View(boatReservationReview);
        }

        // POST: BoatReservationReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatReservationId,Review,DidntShow,ReceivedPenalty,Id,RowVersion")] BoatReservationReview boatReservationReview)
        {
            if (id != boatReservationReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatReservationReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatReservationReviewExists(boatReservationReview.Id))
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
            return View(boatReservationReview);
        }

        // GET: BoatReservationReviews/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReservationReview = await _context.BoatReservationReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReservationReview == null)
            {
                return NotFound();
            }

            return View(boatReservationReview);
        }

        // POST: BoatReservationReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatReservationReview = await _context.BoatReservationReview.FindAsync(id);
            _context.BoatReservationReview.Remove(boatReservationReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatReservationReviewExists(Guid id)
        {
            return _context.BoatReservationReview.Any(e => e.Id == id);
        }
    }
}
