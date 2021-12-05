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
    public class CottageReservationsHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public List<CottageReservation> myCottageReservations { get; set; }

        public CottageReservationsHistoryModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        /*   public void OnGet()
           {
           }*/
        public async Task<IActionResult> OnGetAsync(string sortOrder="")
        {
            var user = await _userManager.GetUserAsync(User);
            myCottageReservations = await _context.CottageReservation.Where(m => m.UserDetailsId == user.Id).ToListAsync();
          //  List<CottageReservations> filteredCottages = new List<Cottage>();

      /*      ViewData["StartDate"] = String.IsNullOrEmpty(sortOrder) ? "StartDate" : "";
            ViewData["EndDate"] = String.IsNullOrEmpty(sortOrder) ? "EndDate" : "";
            ViewData["Price"] = String.IsNullOrEmpty(sortOrder) ? "Address" : "";
            ViewData["MaxPersonCount"] = String.IsNullOrEmpty(sortOrder) ? "MaxPersonCount" : "";
            var ctg = from b in _context.CottageReservation
                      select b;
            switch (sortOrder)
            {
                case "StartDate":
                    ctg = ctg.OrderBy(b => b.StartDate);
                    break;
                case "EndDate":
                    ctg = ctg.OrderBy(b => b.EndDate);
                    break;
                case "Price":
                    ctg = ctg.OrderBy(b => b.Price);
                    break;
                case "MaxPersonCount":
                    ctg = ctg.OrderBy(b => b.MaxPersonCount);
                    break;
     


            }*/
            return Page();
        }
    }
}
