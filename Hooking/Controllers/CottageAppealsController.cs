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
    public class CottageAppealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CottageAppealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CottageAppeals
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageAppeal.ToListAsync());
        }

        // GET: CottageAppeals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageAppeal = await _context.CottageAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageAppeal == null)
            {
                return NotFound();
            }

            return View(cottageAppeal);
        }

        // GET: CottageAppeals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CottageAppeals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/CottageAppeals/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("CottageId,AppealContent,Id,RowVersion")] CottageAppeal cottageAppeal)
        {
            if (ModelState.IsValid)
            {
                cottageAppeal.Id = Guid.NewGuid();
                cottageAppeal.CottageId = id.ToString();
                _context.Add(cottageAppeal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cottageAppeal);
        }

        // GET: CottageAppeals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageAppeal = await _context.CottageAppeal.FindAsync(id);
            if (cottageAppeal == null)
            {
                return NotFound();
            }
            return View(cottageAppeal);
        }

        // POST: CottageAppeals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CottageId,AppealContent,Id,RowVersion")] CottageAppeal cottageAppeal)
        {
            if (id != cottageAppeal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cottageAppeal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageAppealExists(cottageAppeal.Id))
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
            return View(cottageAppeal);
        }

        // GET: CottageAppeals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageAppeal = await _context.CottageAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageAppeal == null)
            {
                return NotFound();
            }

            return View(cottageAppeal);
        }

        // POST: CottageAppeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cottageAppeal = await _context.CottageAppeal.FindAsync(id);
            _context.CottageAppeal.Remove(cottageAppeal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageAppealExists(Guid id)
        {
            return _context.CottageAppeal.Any(e => e.Id == id);
        }
    }
}
