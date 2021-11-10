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
    public class AdventureReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureReviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureReview.ToListAsync());
        }

        // GET: AdventureReviews/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReview = await _context.AdventureReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureReview == null)
            {
                return NotFound();
            }

            return View(adventureReview);
        }

        // GET: AdventureReviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdventureId,UserDetailsId,Review,Grade,IsApproved,Id,RowVersion")] AdventureReview adventureReview)
        {
            if (ModelState.IsValid)
            {
                adventureReview.Id = Guid.NewGuid();
                _context.Add(adventureReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureReview);
        }

        // GET: AdventureReviews/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReview = await _context.AdventureReview.FindAsync(id);
            if (adventureReview == null)
            {
                return NotFound();
            }
            return View(adventureReview);
        }

        // POST: AdventureReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdventureId,UserDetailsId,Review,Grade,IsApproved,Id,RowVersion")] AdventureReview adventureReview)
        {
            if (id != adventureReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureReviewExists(adventureReview.Id))
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
            return View(adventureReview);
        }

        // GET: AdventureReviews/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReview = await _context.AdventureReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureReview == null)
            {
                return NotFound();
            }

            return View(adventureReview);
        }

        // POST: AdventureReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureReview = await _context.AdventureReview.FindAsync(id);
            _context.AdventureReview.Remove(adventureReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureReviewExists(Guid id)
        {
            return _context.AdventureReview.Any(e => e.Id == id);
        }
    }
}
