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
            List<Cottage> myCottages = new List<Cottage>();
            foreach(var myCottageReservation in myCottageReservations )
            {
               
                Cottage ctg = _context.Cottage.Where(m => m.Id == Guid.Parse(myCottageReservation.CottageId)).FirstOrDefault<Cottage>();
                myCottages.Add(ctg);

            }
            ViewData["Cottage"] = myCottages;
            return Page();
        }
    }
}
