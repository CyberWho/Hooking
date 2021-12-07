using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hooking.Models;
using Microsoft.EntityFrameworkCore;

namespace Hooking.Areas.Identity.Pages.Account.Manage
{
    public class CottagesReservationsHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public List<CottageReservation> cottageReservations { get; set; }
        [BindProperty]
        public List<string> cottageNames { get; set; }
        public CottagesReservationsHistoryModel(ApplicationDbContext context,
                                          UserManager<IdentityUser> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          SignInManager<IdentityUser> signInManager,
                                          IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> OnGetAsync(string sortOrder="")
        {
            var user = await _userManager.GetUserAsync(User);
            List<Cottage> myCottages = _context.Cottage.Where(m => m.CottageOwnerId == user.Id).ToList();
            cottageReservations = new List<CottageReservation>();
            cottageNames = new List<string>();
            foreach (var cottage in myCottages)
            {
                var cottageId = cottage.Id.ToString();
                List<CottageReservation> myCottageReservations = new List<CottageReservation>();
                myCottageReservations = _context.CottageReservation.Where(m => m.CottageId == cottageId).ToList();
                if (myCottageReservations.Count != 0)
                {
                    foreach (CottageReservation cottageReservation in myCottageReservations)
                    {
                        if (cottageReservation.StartDate <= DateTime.Now)
                        {
                            cottageReservations.Add(cottageReservation);
                            cottageNames.Add(cottage.Name);
                        }
                    }
                }

            }
           // List<Cottage> cottages = await _context.Cottage.ToListAsync();
           
            ViewData["CottageNames"] = cottageNames;
            return Page();
        }
    }
}
