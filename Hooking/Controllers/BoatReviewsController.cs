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
    public class BoatReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatReviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatReview.ToListAsync());
        }

        // GET: BoatReviews/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReview = await _context.BoatReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReview == null)
            {
                return NotFound();
            }

            return View(boatReview);
        }

        // GET: BoatReviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatId,UserDetailsId,Review,Grade,IsApproved,Id,RowVersion")] BoatReview boatReview)
        {
            if (ModelState.IsValid)
            {
                boatReview.Id = Guid.NewGuid();
                _context.Add(boatReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatReview);
        }

        // GET: BoatReviews/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReview = await _context.BoatReview.FindAsync(id);
            if (boatReview == null)
            {
                return NotFound();
            }
            return View(boatReview);
        }

        // POST: BoatReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatId,UserDetailsId,Review,Grade,IsApproved,Id,RowVersion")] BoatReview boatReview)
        {
            if (id != boatReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatReviewExists(boatReview.Id))
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
            return View(boatReview);
        }

        // GET: BoatReviews/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatReview = await _context.BoatReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatReview == null)
            {
                return NotFound();
            }

            return View(boatReview);
        }

        // POST: BoatReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatReview = await _context.BoatReview.FindAsync(id);
            _context.BoatReview.Remove(boatReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatReviewExists(Guid id)
        {
            return _context.BoatReview.Any(e => e.Id == id);
        }
    }
}
