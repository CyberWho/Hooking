using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Identity;
using Hooking.Models;

namespace Hooking.Controllers
{
    public class CottageNotAvailablePeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Cottage cottage;
        public CottageNotAvailablePeriodsController(ApplicationDbContext context,
                                                    UserManager<IdentityUser> userManager,
                                                    RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: CottageNotAvailablePeriods
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageNotAvailablePeriod.ToListAsync());
        }

        // GET: CottageNotAvailablePeriods/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageNotAvailablePeriod = await _context.CottageNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(cottageNotAvailablePeriod);
        }

        // GET: CottageNotAvailablePeriods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CottageNotAvailablePeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/CottageNotAvailablePeriods/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("StartTime,EndTime,Id,RowVersion")] CottageNotAvailablePeriod cottageNotAvailablePeriod)
        {
            if (ModelState.IsValid)
            {
                cottageNotAvailablePeriod.Id = Guid.NewGuid();
                cottageNotAvailablePeriod.CottageId = id.ToString();
                _context.Add(cottageNotAvailablePeriod);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return View(cottageNotAvailablePeriod);
        }

        // GET: CottageNotAvailablePeriods/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageNotAvailablePeriod = await _context.CottageNotAvailablePeriod.FindAsync(id);
            if (cottageNotAvailablePeriod == null)
            {
                return NotFound();
            }
            return View(cottageNotAvailablePeriod);
        }

        // POST: CottageNotAvailablePeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CottageId,StartTime,EndTime,Id,RowVersion")] CottageNotAvailablePeriod cottageNotAvailablePeriod)
        {
            if (id != cottageNotAvailablePeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cottageNotAvailablePeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageNotAvailablePeriodExists(cottageNotAvailablePeriod.Id))
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
            return View(cottageNotAvailablePeriod);
        }

        // GET: CottageNotAvailablePeriods/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageNotAvailablePeriod = await _context.CottageNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(cottageNotAvailablePeriod);
        }

        // POST: CottageNotAvailablePeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cottageNotAvailablePeriod = await _context.CottageNotAvailablePeriod.FindAsync(id);
            _context.CottageNotAvailablePeriod.Remove(cottageNotAvailablePeriod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageNotAvailablePeriodExists(Guid id)
        {
            return _context.CottageNotAvailablePeriod.Any(e => e.Id == id);
        }
    }
}
