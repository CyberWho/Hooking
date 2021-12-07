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
        public List<BoatReservation> myBoatReservations = new List<BoatReservation>();
        public BoatReservationsHistoryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<BoatReservation> btReservations { get; set; }
        public string StartDateSort { get; set; }
        public string PriceSort { get; set; }
        public async Task<IActionResult> OnGetAsync(string sortOrder="")
        {
            StartDateSort = sortOrder == "StartDate" ? "date_desc" : "StartDate";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            IQueryable<BoatReservation> reservationToSort = from s in _context.BoatReservation
                                                            select s;
            switch (sortOrder)
            {
                case "StartDate":
                    reservationToSort = reservationToSort.OrderBy(s => s.StartDate);
                    break;
                case "date_desc":
                    reservationToSort = reservationToSort.OrderByDescending(s => s.StartDate);
                    break;
                case "Price":
                    reservationToSort = reservationToSort.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    reservationToSort = reservationToSort.OrderByDescending(s => s.Price);
                    break;
            }
            btReservations = await reservationToSort.AsNoTracking().ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            foreach (var btReservation in btReservations)
            {
                if (btReservation.UserDetailsId == user.Id)
                {
                    myBoatReservations.Add(btReservation);
                }
            }

            //  myBoatReservations = await _context.BoatReservation.Where(m => m.UserDetailsId == user.Id).ToListAsync();
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
