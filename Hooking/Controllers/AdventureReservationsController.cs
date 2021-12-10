using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Services;
using Microsoft.AspNetCore.Identity;

namespace Hooking.Controllers
{
    public class AdventureReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdventureService _adventureService;

        public AdventureReservationsController(ApplicationDbContext context, 
            IAdventureService adventureService, 
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _adventureService = adventureService;
        }

        // GET: AdventureReservations
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserDetails userDetails = _context.UserDetails.FirstOrDefault(u => u.IdentityUserId == userId);

            if (userDetails == null)
            {
                return NotFound();
            }

            return View(_adventureService.GetAdventureReservations(userDetails.Id));
        }
        // GET: AdventureReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdventureRealisationId,UserDetailsId,Id,RowVersion")] AdventureReservation adventureReservation)
        {
            if (ModelState.IsValid)
            {
                adventureReservation.Id = Guid.NewGuid();
                _context.Add(adventureReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureReservation);
        }


        // GET: AdventureReservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureReservation = await _context.AdventureReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            var adventureRealisation = await _context.AdventureRealisation.FirstOrDefaultAsync(m => m.Id == Guid.Parse(adventureReservation.AdventureRealisationId));
            ViewData["AdventureRealisation"] = adventureRealisation;
            System.Diagnostics.Debug.WriteLine(adventureRealisation.StartDate.ToString());
            if (adventureReservation == null)
            {
                return NotFound();
            }

            return View(adventureReservation);
        }

        // POST: AdventureReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureReservation = await _context.AdventureReservation.FindAsync(id);
            _context.AdventureReservation.Remove(adventureReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }

}
