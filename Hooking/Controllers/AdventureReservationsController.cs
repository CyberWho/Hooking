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
        [HttpGet]
        public IActionResult CreateView(Guid id, Guid cId)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateView(Guid id, Guid cId, [Bind("Id,RowVersion")] AdventureReservation adventureReservation)
        {
            if (ModelState.IsValid)
            {
                adventureReservation.Id = Guid.NewGuid();
                adventureReservation.AdventureRealisationId = cId.ToString();
                adventureReservation.UserDetailsId = id.ToString();
                adventureReservation.IsReviewed = false;
                await _context.SaveChangesAsync();

                return Redirect("/Adventures/Index");
            }
            return Redirect("/Adventures/Index");

        }
        public async Task<IActionResult> AdventureReservationHistory()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserDetails userDetails = _context.UserDetails.FirstOrDefault(u => u.IdentityUserId == userId);

            if (userDetails == null)
            {
                return NotFound();
            }

            return View(_adventureService.GetAdventureReservationsHistory(userDetails.Id));
        }

    }
        
}
