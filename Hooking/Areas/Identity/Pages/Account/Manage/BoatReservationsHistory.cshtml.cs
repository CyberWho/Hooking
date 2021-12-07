using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Hooking.Areas.Identity.Pages.Account.Manage
{
    public class BoatReservationsHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public List<BoatReservation> myBoatReservations { get; set; }
        public BoatReservationsHistoryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            myBoatReservations = await _context.BoatReservation.Where(m => m.UserDetailsId == user.Id).ToListAsync();
            List<Boat> myBoats = new List<Boat>();
            foreach (var myBoatReservation in myBoatReservations)
            {

                Boat bt = _context.Boat.Where(m => m.Id == Guid.Parse(myBoatReservation.BoatId)).FirstOrDefault<Boat>();
                myBoats.Add(bt);

            }
            ViewData["Boat"] = myBoats;
            return Page();
        }
    }
}
