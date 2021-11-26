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
    public class CancelationPoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CancelationPoliciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CancelationPolicies
        public async Task<IActionResult> Index()
        {
            return View(await _context.CancelationPolicy.ToListAsync());
        }

        // GET: CancelationPolicies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancelationPolicy = await _context.CancelationPolicy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancelationPolicy == null)
            {
                return NotFound();
            }

            return View(cancelationPolicy);
        }

        // GET: CancelationPolicies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CancelationPolicies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("CancelationPolicies/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("FreeUntil,PenaltyPercentage,Id,RowVersion")] CancelationPolicy cancelationPolicy)
        {
            if (ModelState.IsValid)
            {
                cancelationPolicy.Id = Guid.NewGuid();
                _context.Add(cancelationPolicy);
                await _context.SaveChangesAsync();
                var cottage = _context.Cottage.Where(m => m.Id == id).FirstOrDefault<Cottage>();
                cottage.CancelationPolicyId = cancelationPolicy.Id.ToString();
                _context.Update(cottage);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return View(cancelationPolicy);
        }

        // GET: CancelationPolicies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancelationPolicy = await _context.CancelationPolicy.FindAsync(id);
            if (cancelationPolicy == null)
            {
                return NotFound();
            }
            return View(cancelationPolicy);
        }

        // POST: CancelationPolicies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FreeUntil,PenaltyPercentage,Id,RowVersion")] CancelationPolicy cancelationPolicy)
        {
            if (id != cancelationPolicy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cancelationPolicy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancelationPolicyExists(cancelationPolicy.Id))
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
            return View(cancelationPolicy);
        }

        // GET: CancelationPolicies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancelationPolicy = await _context.CancelationPolicy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cancelationPolicy == null)
            {
                return NotFound();
            }

            return View(cancelationPolicy);
        }

        // POST: CancelationPolicies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cancelationPolicy = await _context.CancelationPolicy.FindAsync(id);
            _context.CancelationPolicy.Remove(cancelationPolicy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CancelationPolicyExists(Guid id)
        {
            return _context.CancelationPolicy.Any(e => e.Id == id);
        }
    }
}
