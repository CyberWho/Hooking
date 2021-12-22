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
using Hooking.Services;
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
        private readonly ICottageReservationsService _cottageReservationsService;
        private readonly ICottageService _cottageService;
        public CottagesReservationsHistoryModel(ApplicationDbContext context,
                                          UserManager<IdentityUser> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          SignInManager<IdentityUser> signInManager,
                                          IEmailSender emailSender,
                                          ICottageReservationsService cottageReservationsService,
                                          ICottageService cottageService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cottageReservationsService = cottageReservationsService;
            _cottageService = cottageService;
        }
        public async Task<IActionResult> OnGetAsync(string sortOrder="")
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Cottage> myCottages = _cottageService.GetAllByOwnerId(user.Id);
            cottageReservations = _cottageReservationsService.GetAllFutureByOwnerId(user.Id, myCottages).ToList();
            cottageNames = new List<string>();
            foreach (CottageReservation cottageReservation in cottageReservations)
            {
                Guid id = Guid.Parse(cottageReservation.CottageId);
                Cottage cottage = _context.Cottage.Where(m => m.Id == id).FirstOrDefault();
                cottageNames.Add(cottage.Name);
            }
            ViewData["CottageNames"] = cottageNames;
            return Page();
        }
    }
}
