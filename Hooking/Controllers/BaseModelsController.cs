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
    public class BaseModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BaseModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.BaseModel.ToListAsync());
        }

        // GET: BaseModels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseModel = await _context.BaseModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseModel == null)
            {
                return NotFound();
            }

            return View(baseModel);
        }

        // GET: BaseModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BaseModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RowVersion")] BaseModel baseModel)
        {
            if (ModelState.IsValid)
            {
                baseModel.Id = Guid.NewGuid();
                _context.Add(baseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baseModel);
        }

        // GET: BaseModels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseModel = await _context.BaseModel.FindAsync(id);
            if (baseModel == null)
            {
                return NotFound();
            }
            return View(baseModel);
        }

        // POST: BaseModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RowVersion")] BaseModel baseModel)
        {
            if (id != baseModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseModelExists(baseModel.Id))
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
            return View(baseModel);
        }

        // GET: BaseModels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseModel = await _context.BaseModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseModel == null)
            {
                return NotFound();
            }

            return View(baseModel);
        }

        // POST: BaseModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var baseModel = await _context.BaseModel.FindAsync(id);
            _context.BaseModel.Remove(baseModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseModelExists(Guid id)
        {
            return _context.BaseModel.Any(e => e.Id == id);
        }
    }
}
