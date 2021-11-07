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
    public class HouseRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HouseRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HouseRules
        public async Task<IActionResult> Index()
        {
            return View(await _context.HouseRules.ToListAsync());
        }

        // GET: HouseRules/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseRules = await _context.HouseRules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseRules == null)
            {
                return NotFound();
            }

            return View(houseRules);
        }

        // GET: HouseRules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HouseRules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetFriendly,NonSmoking,CheckinTime,CheckoutTime,AgeRestriction,Id,RowVersion")] HouseRules houseRules)
        {
            if (ModelState.IsValid)
            {
                houseRules.Id = Guid.NewGuid();
                _context.Add(houseRules);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(houseRules);
        }

        // GET: HouseRules/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseRules = await _context.HouseRules.FindAsync(id);
            if (houseRules == null)
            {
                return NotFound();
            }
            return View(houseRules);
        }

        // POST: HouseRules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PetFriendly,NonSmoking,CheckinTime,CheckoutTime,AgeRestriction,Id,RowVersion")] HouseRules houseRules)
        {
            if (id != houseRules.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(houseRules);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseRulesExists(houseRules.Id))
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
            return View(houseRules);
        }

        // GET: HouseRules/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var houseRules = await _context.HouseRules
                .FirstOrDefaultAsync(m => m.Id == id);
            if (houseRules == null)
            {
                return NotFound();
            }

            return View(houseRules);
        }

        // POST: HouseRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var houseRules = await _context.HouseRules.FindAsync(id);
            _context.HouseRules.Remove(houseRules);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HouseRulesExists(Guid id)
        {
            return _context.HouseRules.Any(e => e.Id == id);
        }
    }
}
