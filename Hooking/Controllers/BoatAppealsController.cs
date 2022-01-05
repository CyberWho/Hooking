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
    public class BoatAppealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatAppealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatAppeals
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatAppeal.ToListAsync());
        }

        // GET: BoatAppeals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatAppeal = await _context.BoatAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatAppeal == null)
            {
                return NotFound();
            }

            return View(boatAppeal);
        }

        // GET: BoatAppeals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatAppeals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/BoatAppeals/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("BoatId,AppealContent,Id,RowVersion")] BoatAppeal boatAppeal)
        {
            if (ModelState.IsValid)
            {
                boatAppeal.Id = Guid.NewGuid();
                boatAppeal.BoatId = id.ToString();
                _context.Add(boatAppeal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatAppeal);
        }

        // GET: BoatAppeals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatAppeal = await _context.BoatAppeal.FindAsync(id);
            if (boatAppeal == null)
            {
                return NotFound();
            }
            return View(boatAppeal);
        }

        // POST: BoatAppeals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoatId,AppealContent,Id,RowVersion")] BoatAppeal boatAppeal)
        {
            if (id != boatAppeal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatAppeal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatAppealExists(boatAppeal.Id))
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
            return View(boatAppeal);
        }

        // GET: BoatAppeals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatAppeal = await _context.BoatAppeal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatAppeal == null)
            {
                return NotFound();
            }

            return View(boatAppeal);
        }

        // POST: BoatAppeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatAppeal = await _context.BoatAppeal.FindAsync(id);
            _context.BoatAppeal.Remove(boatAppeal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatAppealExists(Guid id)
        {
            return _context.BoatAppeal.Any(e => e.Id == id);
        }
    }
}
