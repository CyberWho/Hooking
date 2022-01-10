using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;

namespace Hooking.Controllers
{
    public class AdventureSpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdventureService _adventureService;
        private readonly IEmailSender _emailSender;
        public AdventureSpecialOffersController(ApplicationDbContext context,
                                                UserManager<IdentityUser> userManager, 
                                                IAdventureService adventureService, 
                                                IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _adventureService = adventureService;
            _emailSender = emailSender;

            using StreamReader reader = new StreamReader("./Data/emailCredentials.json");
            string json = reader.ReadToEnd();
            _emailSender = JsonConvert.DeserializeObject<EmailSender>(json);
        }

        // GET: AdventureSpecialOffers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            Guid userId = Guid.Parse(user.Id);
            System.Diagnostics.Debug.WriteLine(userId);
            UserDetails userDetails = await _context.UserDetails.Where(m => m.IdentityUserId == user.Id).FirstOrDefaultAsync<UserDetails>();
            var userDetailsId = userDetails.Id.ToString();
            Instructor instructor = await _context.Instructor.Where(m => m.UserDetailsId == userDetailsId).FirstOrDefaultAsync<Instructor>();
            string instructorId = instructor.Id.ToString();
            List<Adventure> adventures = await _context.Adventure.Where(m => m.InstructorId == instructorId).ToListAsync<Adventure>();
            List<AdventureSpecialOffer> adventureSpecialOffers = new List<AdventureSpecialOffer>();
            List<string> adventureNames = new List<string>();
            foreach(Adventure adventure in adventures)
            {
                string adventureId = adventure.Id.ToString();
                List<AdventureSpecialOffer> adventureSpecials = await _context.AdventureSpecialOffer.Where(m => m.AdventureId == adventureId).ToListAsync<AdventureSpecialOffer>();
                adventureSpecialOffers.AddRange(adventureSpecials);
                for(int i = 0; i < adventureSpecials.Count; i++)
                    adventureNames.Add(adventure.Name);
            }
            ViewData["AdventureNames"] = adventureNames;
            return View(adventureSpecialOffers);
        }
       
        // GET: AdventureSpecialOffers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }

            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdventureSpecialOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("AdventureSpecialOffers/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("StartDate,Duration,Price,MaxPersonCount,Description,Id,RowVersion")] AdventureSpecialOffer adventureSpecialOffer)
        {
            if (ModelState.IsValid)
            {
                adventureSpecialOffer.Id = Guid.NewGuid();
                adventureSpecialOffer.AdventureId = id.ToString();
                adventureSpecialOffer.IsReserved = false;
                _context.Add(adventureSpecialOffer);


                Adventure adventure = _context.Adventure.Find(id);

                var favorites = _context.AdventureFavorites.Where(f =>
                    f.AdventureId == id.ToString()).ToList();

                foreach (AdventureFavorites favorite in favorites)
                {
                    UserDetails user = _context.UserDetails.Find(Guid.Parse(favorite.UserDetailsId));
                    IdentityUser iUser = await _userManager.FindByIdAsync(user.IdentityUserId);
                    await _emailSender.SendEmailAsync(iUser.Email, "Avantura koju pratite je na akciji", $"Avanatura {adventure.Name} je na specijalnoj akciji po ceni od {adventureSpecialOffer.Price} za najviše {adventureSpecialOffer.MaxPersonCount} ljudi, a sa početkom {adventureSpecialOffer.StartDate}!\n\nNe propustite ovu sjajnu priliku!");
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer.FindAsync(id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }
            return View(adventureSpecialOffer);
        }

        // POST: AdventureSpecialOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserDetailsId,Description,Id,RowVersion")] AdventureSpecialOffer adventureSpecialOffer)
        {
            if (id != adventureSpecialOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adventureSpecialOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureSpecialOfferExists(adventureSpecialOffer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adventureSpecialOffer);
        }

        // GET: AdventureSpecialOffers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventureSpecialOffer = await _context.AdventureSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adventureSpecialOffer == null)
            {
                return NotFound();
            }

            return View(adventureSpecialOffer);
        }

        // POST: AdventureSpecialOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventureSpecialOffer = await _context.AdventureSpecialOffer.FindAsync(id);
            _context.AdventureSpecialOffer.Remove(adventureSpecialOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdventureSpecialOfferExists(Guid id)
        {
            return _context.AdventureSpecialOffer.Any(e => e.Id == id);
        }
    }
}
