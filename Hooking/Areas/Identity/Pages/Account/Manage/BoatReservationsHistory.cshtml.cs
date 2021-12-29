using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Hooking.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hooking.Areas.Identity.Pages.Account.Manage
{
    public class BoatReservationsHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public List<BoatReservation> boatReservations { get; set; }
        public List<string> boatNames { get; set; } 
        public BoatReservationsHistoryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Boat> boats = _context.Boat.Where(m => m.BoatOwnerId == user.Id).ToList<Boat>();
            boatReservations = new List<BoatReservation>();
            boatNames = new List<string>();
            foreach(Boat boat in boats)
            {
                string boatId = boat.Id.ToString();
                List<BoatReservation> myBoatReservations = _context.BoatReservation.Where(m => m.BoatId == boatId).ToList<BoatReservation>();
                if(myBoatReservations.Count != 0)
                {
                    foreach(BoatReservation boatReservation in myBoatReservations)
                    {
                        if(boatReservation.StartDate <= DateTime.Now)
                        {
                            boatReservations.Add(boatReservation);
                            boatNames.Add(boat.Name);
                        }
                    }
                }
            }
            ViewData["BoatNames"] = boatNames;
            return Page();
        }
    }
}
