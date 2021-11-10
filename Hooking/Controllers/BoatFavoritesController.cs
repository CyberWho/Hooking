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
    public class BoatFavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatFavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BoatFavorites
        public async Task<IActionResult> Index()
        {
            return View(await _context.BoatFavorites.ToListAsync());
        }

        // GET: BoatFavorites/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatFavorites = await _context.BoatFavorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatFavorites == null)
            {
                return NotFound();
            }

            return View(boatFavorites);
        }

        // GET: BoatFavorites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoatFavorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserDetailsId,BoatId,Id,RowVersion")] BoatFavorites boatFavorites)
        {
            if (ModelState.IsValid)
            {
                boatFavorites.Id = Guid.NewGuid();
                _context.Add(boatFavorites);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatFavorites);
        }

        // GET: BoatFavorites/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatFavorites = await _context.BoatFavorites.FindAsync(id);
            if (boatFavorites == null)
            {
                return NotFound();
            }
            return View(boatFavorites);
        }

        // POST: BoatFavorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserDetailsId,BoatId,Id,RowVersion")] BoatFavorites boatFavorites)
        {
            if (id != boatFavorites.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatFavorites);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatFavoritesExists(boatFavorites.Id))
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
            return View(boatFavorites);
        }

        // GET: BoatFavorites/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatFavorites = await _context.BoatFavorites
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boatFavorites == null)
            {
                return NotFound();
            }

            return View(boatFavorites);
        }

        // POST: BoatFavorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boatFavorites = await _context.BoatFavorites.FindAsync(id);
            _context.BoatFavorites.Remove(boatFavorites);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatFavoritesExists(Guid id)
        {
            return _context.BoatFavorites.Any(e => e.Id == id);
        }
    }
}
