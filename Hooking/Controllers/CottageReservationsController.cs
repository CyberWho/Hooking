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

namespace Hooking.Controllers
{
    public class CottageReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Cottage cottage;
        public UserDetails userDetails;

        public CottageReservationsController(ApplicationDbContext context, 
                                             UserManager<IdentityUser> userManager,
                                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: CottageReservations
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var reservations = await _context.CottageReservation.Where(m => m.UserDetailsId==user.Id).ToListAsync();
           
           // return View(await _context.CottageReservation.ToListAsync());
           return View(reservations);
        }

        // GET: CottageReservationsHistory
        public async Task<IActionResult> CottageReservationsHistory(string sortOrder="")
        {

            var user = await _userManager.GetUserAsync(User);
            var reservations = await _context.CottageReservation.Where(m => m.UserDetailsId == user.Id).ToListAsync();

            List<CottageReservation> cottageReservations = await _context.CottageReservation.ToListAsync();
            

            ViewData["StartDate"] = String.IsNullOrEmpty(sortOrder) ? "StartDate" : "";
            ViewData["EndDate"] = String.IsNullOrEmpty(sortOrder) ? "EndDate" : "";
            ViewData["Price"] = String.IsNullOrEmpty(sortOrder) ? "Price" : "";
   
            var ctg = from b in cottageReservations
                      select b;
            switch (sortOrder)
            {
                case "StartDate":
                    ctg = ctg.OrderBy(b => b.StartDate);
                    break;
                case "Address":
                    ctg = ctg.OrderBy(b => b.EndDate);
                    break;
                case "City":
                    ctg = ctg.OrderBy(b => b.Price);
                    break;
            }
          

            // return View(await _context.CottageReservation.ToListAsync());
            return View(ctg);
        }
        // GET : CottageReservations/CreateReservation/5/3
        public IActionResult CreateReservation(Guid? id, Guid? cId)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (cId == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost("/CottageReservations/CreateReservation/{id}/{cId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReservation(Guid id, Guid cId, [Bind("StartDate,EndDate,PersonCount,Id,RowVersion")] CottageReservation cottageReservation)
        {
            if (ModelState.IsValid)
            {
                cottageReservation.Id = Guid.NewGuid();
                cottageReservation.Price = 1000;
                cottageReservation.UserDetailsId = id.ToString();
                cottageReservation.CottageId = cId.ToString();
                _context.Add(cottageReservation);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottageReservations", new { area = "Identity" });
            }
            return RedirectToPage("/Account/Manage/MyCottageReservations", new { area = "Identity" });

        }
        // GET: CottageReservations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageReservation = await _context.CottageReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            Guid cottageId = Guid.Parse(cottageReservation.CottageId);
            cottage = _context.Cottage.Where(m => m.Id == cottageId).FirstOrDefault<Cottage>();
            userDetails = _context.UserDetails.Where(m => m.IdentityUserId == cottageReservation.UserDetailsId).FirstOrDefault<UserDetails>();
            Guid identityUserId = Guid.Parse(userDetails.IdentityUserId);
            var identityUser = _context.Users.Where(m => m.Id == userDetails.IdentityUserId).FirstOrDefault();
            string email = identityUser.Email;
            string phoneNumber = identityUser.PhoneNumber;
            if (cottageReservation == null)
            {
                return NotFound();
            }
            ViewData["Cottage"] = cottage;
            ViewData["UserDetails"] = userDetails;
            ViewData["Email"] = email;
            ViewData["PhoneNumber"] = phoneNumber;
            return View(cottageReservation);
        }

        // GET: CottageReservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CottageReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CottageId,UserDetailsId,StartDate,EndDate,Price,PersonCount,Id,RowVersion")] CottageReservation cottageReservation)
        {
            if (ModelState.IsValid)
            {
                cottageReservation.Id = Guid.NewGuid();
                _context.Add(cottageReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cottageReservation);
        }

        // GET: CottageReservations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageReservation = await _context.CottageReservation.FindAsync(id);
            if (cottageReservation == null)
            {
                return NotFound();
            }
            return View(cottageReservation);
        }

        // POST: CottageReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CottageId,UserDetailsId,StartDate,EndDate,Price,PersonCount,Id,RowVersion")] CottageReservation cottageReservation)
        {
            if (id != cottageReservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cottageReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageReservationExists(cottageReservation.Id))
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
            return View(cottageReservation);
        }

        // GET: CottageReservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageReservation = await _context.CottageReservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageReservation == null)
            {
                return NotFound();
            }

            return View(cottageReservation);
        }

        // POST: CottageReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cottageReservation = await _context.CottageReservation.FindAsync(id);
            _context.CottageReservation.Remove(cottageReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageReservationExists(Guid id)
        {
            return _context.CottageReservation.Any(e => e.Id == id);
        }
    }
}
