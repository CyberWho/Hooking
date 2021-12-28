using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Hooking.Services;

namespace Hooking.Controllers
{
    public class CottageSpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ICottageSpecialOffersService _cottageSpecialOffersService;
        private readonly ICottageService _cottageService;
        private readonly ICottageFavoritesService _cottageFavoritesService;
        public Cottage cottage;

        public CottageSpecialOffersController(ApplicationDbContext context,
                                              UserManager<IdentityUser> userManager,
                                              RoleManager<IdentityRole> roleManager,
                                              IEmailSender emailSender,
                                              ICottageSpecialOffersService cottageSpecialOffersService,
                                              ICottageService cottageService,
                                              ICottageFavoritesService cottageFavoritesService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _cottageSpecialOffersService = cottageSpecialOffersService;
            _cottageService = cottageService;
            _cottageFavoritesService = cottageFavoritesService;
            using (StreamReader reader = new StreamReader("./Data/emailCredentials.json"))
            {
                string json = reader.ReadToEnd();
                _emailSender = JsonConvert.DeserializeObject<EmailSender>(json);
            }
        }

        // GET: CottageSpecialOffers
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageSpecialOffer.ToListAsync());
        }

        // GET: CottageSpecialOffers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = _cottageSpecialOffersService.GetById(id);
            Guid cottageId = Guid.Parse(cottageSpecialOffer.CottageId);
            cottage = _cottageService.GetCottageById(cottageId);
            
            if (cottageSpecialOffer == null)
            {
                return NotFound();
            }
            ViewData["Cottage"] = cottage; 
            return View(cottageSpecialOffer);
        }

        // GET: CottageSpecialOffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CottageSpecialOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/CottageSpecialOffers/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("StartDate,EndDate,Price,MaxPersonCount,Description,Id,RowVersion")] CottageSpecialOffer cottageSpecialOffer)
        {
            if (ModelState.IsValid)
            {
                cottageSpecialOffer.Id = Guid.NewGuid();
                cottageSpecialOffer.CottageId = id.ToString();
                cottageSpecialOffer.StartDate = cottageSpecialOffer.StartDate.Date;
                cottageSpecialOffer.EndDate = cottageSpecialOffer.EndDate.Date;
                cottageSpecialOffer.IsReserved = false;
                _cottageSpecialOffersService.Create(cottageSpecialOffer);
                Cottage cottage = _cottageService.GetCottageById(id);
                List<CottageFavorites> cottageFavorites = _cottageFavoritesService.GetAllByCottageId(cottage.Id).ToList();
                foreach (var subscribe in cottageFavorites)
                {
                    UserDetails userDetails = _context.UserDetails.Where(m => m.IdentityUserId == subscribe.UserDetailsId).FirstOrDefault<UserDetails>();
                    var user = await _context.Users.FindAsync(userDetails.IdentityUserId);
                    var callbackUrl = Url.Action( "Details", "CottageSpecialOffers", new { id = cottageSpecialOffer.Id });

                    await _emailSender.SendEmailAsync(user.Email, "Obaveštenje o specijalnoj akciji",
                               $"Poštovani,<br><br> upravo je objavljena specijalna akcija za vikendicu na koju ste pretplaćeni! Za više detalja kliknite na sledeći link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ovaj link</a>.");

                }
                return RedirectToPage("/Account/Manage/MySpecialOffers", new { area = "Identity"});
            }
            return View(cottageSpecialOffer);
        }
        private async void SendNotificationToSubscribers(Guid cottageId, Guid specialOfferId)
        {
            Cottage cottage = _context.Cottage.Where(m => m.Id == cottageId).FirstOrDefault();
            var cId = cottageId.ToString();
            List<CottageFavorites> cottageFavorites = new List<CottageFavorites>();
            cottageFavorites = _context.CottageFavorites.Where(m => m.CottageId == cId).ToList();
            foreach(var subscribe in cottageFavorites)
            {
                UserDetails userDetails = _context.UserDetails.Where(m => m.IdentityUserId == subscribe.UserDetailsId).FirstOrDefault<UserDetails>();
                var user = await _context.Users.FindAsync(userDetails.IdentityUserId);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                           "/CottageSpecialOffers/Details",
                           pageHandler: null,
                           values: new { id = specialOfferId },
                           protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Obaveštenje o specijalnoj akciji",
                           $"Poštovani,<br><br> upravo je objavljena specijalna akcija za vikendicu na koju ste pretplaćeni! Za više detalja kliknite na sledeći link <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ovaj link</a>.");

            }
        }
        // GET: CottageSpecialOffers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = _cottageSpecialOffersService.GetById(id);
            if (cottageSpecialOffer == null)
            {
                return NotFound();
            }
            return View(cottageSpecialOffer);
        }

        // POST: CottageSpecialOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CottageId,StartDate,EndDate,Price,MaxPersonCount,Description,IsReserved,Id,RowVersion")] CottageSpecialOffer cottageSpecialOffer)
        {
            if (id != cottageSpecialOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _cottageSpecialOffersService.Update(id, cottageSpecialOffer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageSpecialOfferExists(cottageSpecialOffer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("/Account/Manage/MySpecialOffers", new { area = "Identity" });
            }
            return View(cottageSpecialOffer);
        }

        // GET: CottageSpecialOffers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = _cottageSpecialOffersService.GetById(id);
            if (cottageSpecialOffer == null)
            {
                return NotFound();
            }

            return View(cottageSpecialOffer);
        }

        // POST: CottageSpecialOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            _cottageSpecialOffersService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CottageSpecialOfferExists(Guid id)
        {
            return _context.CottageSpecialOffer.Any(e => e.Id == id);
        }
    }
}
