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

namespace Hooking.Areas.Identity.Pages.Account.Manage
{
    public class MyCottageReservationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public List<CottageReservation> cottageReservations { get; set; }
        public MyCottageReservationsModel(ApplicationDbContext context,
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
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Cottage> myCottages = _context.Cottage.Where(m => m.CottageOwnerId == user.Id).ToList();
            cottageReservations = new List<CottageReservation>();
            foreach(var cottage in myCottages)
            {
                var cottageId = cottage.Id.ToString();
                List<CottageReservation> myCottageReservations = new List<CottageReservation>();
                myCottageReservations = _context.CottageReservation.Where(m => m.CottageId == cottageId).ToList();
                if(myCottageReservations.Count != 0)
                {
                    cottageReservations.AddRange(myCottageReservations);
                }
                    
            }
            return Page();
        }
    }
}
