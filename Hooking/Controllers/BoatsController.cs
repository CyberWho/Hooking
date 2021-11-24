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

namespace Hooking.Controllers
{
    public class BoatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BoatsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Boats
        public async Task<IActionResult> Index(string searchString = "")
        {
            return View(await _context.Boat.ToListAsync());
        }
        //GET : BoatOwner/Details/5/ShowMyBoats
        public async Task<IActionResult> ShowMyBoats()
        {
            var user = await _userManager.GetUserAsync(User);
            var boats = await _context.Boat.Where(m => m.BoatOwnerId == user.Id).ToListAsync();
            return View(boats);
        }

        // GET: Boats/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }

        // GET: Boats/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult BoatSpecialOffers()
        {
            return Redirect("/BoatSpecialOffers");
        }

        // POST: Boats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type,Length,Capacity,EngineNumber,EnginePower,MaxSpeed,Address,City,Country,CancelationPolicyId,Description,AverageGrade,GradeCount,RegularPrice,WeekendPrice,BoatOwnerId,Id,RowVersion")] Boat boat)
        {
            if (ModelState.IsValid)
            {
                boat.Id = Guid.NewGuid();
                var user = await _userManager.GetUserAsync(User);
                boat.BoatOwnerId = user.Id;
                boat.CancelationPolicyId = "0";
                boat.AverageGrade = 0;
                boat.GradeCount = 0;
                _context.Add(boat);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyBoats", new { area = "Identity" });
            }
            return View(boat);
        }

        // GET: Boats/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat.FindAsync(id);
            if (boat == null)
            {
                return NotFound();
            }
            return View(boat);
        }

        // POST: Boats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Type,Length,Capacity,EngineNumber,EnginePower,MaxSpeed,Address,City,Country,CancelationPolicyId,Description,AverageGrade,GradeCount,RegularPrice,WeekendPrice,BoatOwnerId,Id,RowVersion")] Boat boat)
        {
            if (id != boat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatExists(boat.Id))
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
            return View(boat);
        }

        // GET: Boats/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }

        // POST: Boats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boat = await _context.Boat.FindAsync(id);
            _context.Boat.Remove(boat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatExists(Guid id)
        {
            return _context.Boat.Any(e => e.Id == id);
        }
    }
}
