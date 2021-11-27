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
    public class CottageSpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CottageSpecialOffersController(ApplicationDbContext context,
                                              UserManager<IdentityUser> userManager,
                                              RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: CottageSpecialOffers
        public async Task<IActionResult> Index()
        {
            return View(await _context.CottageSpecialOffer.ToListAsync());
        }

        // GET: CottageSpecialOffers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = await _context.CottageSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottageSpecialOffer == null)
            {
                return NotFound();
            }

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
                _context.Add(cottageSpecialOffer);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MySpecialOffers", new { area = "Identity"});
            }
            return View(cottageSpecialOffer);
        }

        // GET: CottageSpecialOffers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = await _context.CottageSpecialOffer.FindAsync(id);
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
                    var cottageSpecialOfferTmp = await _context.CottageSpecialOffer.FindAsync(id);
                    cottageSpecialOfferTmp.Id = id;
                    cottageSpecialOfferTmp.CottageId = cottageSpecialOffer.CottageId;
                    cottageSpecialOfferTmp.StartDate = cottageSpecialOffer.StartDate;
                    cottageSpecialOfferTmp.EndDate = cottageSpecialOffer.EndDate;
                    cottageSpecialOfferTmp.Price = cottageSpecialOffer.Price;
                    cottageSpecialOfferTmp.MaxPersonCount = cottageSpecialOffer.MaxPersonCount;
                    cottageSpecialOfferTmp.Description = cottageSpecialOffer.Description;
                    cottageSpecialOfferTmp.IsReserved = cottageSpecialOffer.IsReserved;
                    _context.Update(cottageSpecialOfferTmp);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottageSpecialOffer = await _context.CottageSpecialOffer
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var cottageSpecialOffer = await _context.CottageSpecialOffer.FindAsync(id);
            _context.CottageSpecialOffer.Remove(cottageSpecialOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageSpecialOfferExists(Guid id)
        {
            return _context.CottageSpecialOffer.Any(e => e.Id == id);
        }
    }
}
