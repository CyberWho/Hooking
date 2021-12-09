using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Services;

namespace Hooking.Controllers
{
    public class AdventuresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdventureService _adventureService;

        public AdventuresController(ApplicationDbContext context, 
            IAdventureService adventureService)
        {
            _context = context;
            _adventureService = adventureService;
        }

        // GET: Adventures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Adventure.ToListAsync());
        }

        public IActionResult InstructorIndex(Guid instructorId)
        {
            return View(_adventureService.GetInstructorAdventures(instructorId));
        }

        // GET: Adventures/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventure = await _context.Adventure
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventure == null)
            {
                return NotFound();
            }

            return View(adventure);
        }

        // GET: Adventures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adventures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,Address,City,Country,Description,MaxPersonCount,CancellationPolicyId,AverageGrade,Id,RowVersion")] Adventure adventure)
        {
            if (ModelState.IsValid)
            {
                adventure.Id = Guid.NewGuid();
                _context.Add(adventure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventure);
        }

        // GET: Adventures/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventure = await _context.Adventure.FindAsync(id);
            if (adventure == null)
            {
                return NotFound();
            }
            return View(adventure);
        }

        // POST: Adventures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InstructorId,Name,Address,City,Country,Description,MaxPersonCount,CancellationPolicyId,AverageGrade,Id,RowVersion")] Adventure adventure)
        {
            if (id != adventure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureExists(adventure.Id))
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
            return View(adventure);
        }

        // GET: Adventures/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventure = await _context.Adventure
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventure == null)
            {
                return NotFound();
            }

            return View(adventure);
        }

        // POST: Adventures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventure = await _context.Adventure.FindAsync(id);
            _context.Adventure.Remove(adventure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureExists(Guid id)
        {
            return _context.Adventure.Any(e => e.Id == id);
        }
    }
}
