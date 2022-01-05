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
    public class AdventureAppealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdventureAppealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdventureAppeals
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdventureAppeal.ToListAsync());
        }

        // GET: AdventureAppeals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureAppeal = await _context.AdventureAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureAppeal == null)
            {
                return NotFound();
            }

            return View(adventureAppeal);
        }

        // GET: AdventureAppeals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureAppeals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/AdventureAppeals/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("AdventureId,AppealContent,Id,RowVersion")] AdventureAppeal adventureAppeal)
        {
            if (ModelState.IsValid)
            {
                adventureAppeal.Id = Guid.NewGuid();
                adventureAppeal.AdventureId = id.ToString();

                _context.Add(adventureAppeal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureAppeal);
        }

        // GET: AdventureAppeals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureAppeal = await _context.AdventureAppeal.FindAsync(id);
            if (adventureAppeal == null)
            {
                return NotFound();
            }
            return View(adventureAppeal);
        }

        // POST: AdventureAppeals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdventureId,AppealContent,Id,RowVersion")] AdventureAppeal adventureAppeal)
        {
            if (id != adventureAppeal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureAppeal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureAppealExists(adventureAppeal.Id))
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
            return View(adventureAppeal);
        }

        // GET: AdventureAppeals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureAppeal = await _context.AdventureAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureAppeal == null)
            {
                return NotFound();
            }

            return View(adventureAppeal);
        }

        // POST: AdventureAppeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureAppeal = await _context.AdventureAppeal.FindAsync(id);
            _context.AdventureAppeal.Remove(adventureAppeal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureAppealExists(Guid id)
        {
            return _context.AdventureAppeal.Any(e => e.Id == id);
        }
    }
}
