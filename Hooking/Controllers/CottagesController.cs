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
using Newtonsoft.Json;
using Hooking.Areas.Identity.Pages.Account.Manage;

namespace Hooking.Controllers
{
    public class CottagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserDetails cottageOwner;
        public CancelationPolicy cancelationPolicy;
        public HouseRules houseRules;
        public CottagesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        [HttpGet("/Cottages/Details/{id}/AddHouseRules")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHouseRules(Guid id, [Bind("PetFriendly,NonSmoking,CheckInTime,CheckOutTime,AgeRestriction,Id,RowVersion")] HouseRules houseRules)
        {
            if (ModelState.IsValid)
            {
                houseRules.Id = Guid.NewGuid();
                _context.Add(houseRules);
                await _context.SaveChangesAsync();
                CottagesHouseRules cottagesHouseRules = new CottagesHouseRules();
                cottagesHouseRules.Id = Guid.NewGuid();
                cottagesHouseRules.CottageId = id.ToString();
                cottagesHouseRules.HouseRulesId = houseRules.Id.ToString();
                _context.Add(cottagesHouseRules);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return View(houseRules);
        }
       

        // GET: Cottages
        public async Task<IActionResult> Index(string searchString = "", string filter = "")
        {
            List<Cottage> cottages = await _context.Cottage.ToListAsync();
            List<Cottage> filteredCottages = new List<Cottage>();

            if (string.IsNullOrEmpty(searchString))
            {
                filteredCottages = cottages;
            }
            else foreach (var cottage in cottages)
                {
                    var json = JsonConvert.SerializeObject(cottage);
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (filter == "Name")
                    {
                        if (cottage.Name.ToString().ToUpper().Contains(searchString.ToUpper()))
                        {
                            filteredCottages.Add(cottage);
                        }
                    }
                    if (filter == "City")
                    {
                        if (cottage.City.ToString().ToUpper().Contains(searchString.ToUpper()))
                        {
                            filteredCottages.Add(cottage);
                        }
                    }
                    if (filter == "Country")
                    {
                        if (cottage.Country.ToString().ToUpper().Contains(searchString.ToUpper()))
                        {
                            filteredCottages.Add(cottage);
                        }
                    }

                }


            return View(filteredCottages);
        }

        // GET: Cottages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.Id == id);
            Guid cottageOwnerId = Guid.Parse(cottage.CottageOwnerId);
            var cottageOwnerUser = _context.CottageOwner.Where(m => m.UserDetailsId == cottage.CottageOwnerId).FirstOrDefault<CottageOwner>();
           
            cottageOwner = _context.UserDetails.Where(m => m.IdentityUserId == cottageOwnerUser.UserDetailsId).FirstOrDefault<UserDetails>();
            var cottageId = cottage.Id.ToString();
            CottagesHouseRules cottagesHouseRules = _context.CottagesHouseRules.Where(m => m.CottageId == cottageId).FirstOrDefault<CottagesHouseRules>();
            Guid houseRulesId = Guid.Parse(cottagesHouseRules.HouseRulesId);
            houseRules = _context.HouseRules.Where(m => m.Id == houseRulesId).FirstOrDefault<HouseRules>();
            if (cottage == null)
            {
                return NotFound();
            }
            ViewData["CottageOwner"] = cottageOwner;
            ViewData["HouseRules"] = houseRules;
            return View(cottage);
        }

        // GET: Cottages/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult CottageSpecialOffers()
        {
            return Redirect("/CottageSpecialOffers");
        }

        // POST: Cottages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,City,Country,Description,RoomCount,Area,AverageGrade,GradeCount,CancelationPolicyId,RegularPrice,WeekendPrice,CottageOwnerId,Id,RowVersion")] Cottage cottage)
        {
            if (ModelState.IsValid)
            {
                cottage.Id = Guid.NewGuid();
                var user = await _userManager.GetUserAsync(User);
                cottage.CottageOwnerId = user.Id;
                cottage.CancelationPolicyId = "0";
                cottage.AverageGrade = 0;
                cottage.GradeCount = 0;
                _context.Add(cottage);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
        }

        // GET: Cottages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage.FindAsync(id);
            if (cottage == null)
            {
                return NotFound();
            }
            return View(cottage);
        }
       
        // POST: Cottages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Description,RoomCount,RegularPrice,WeekendPrice,Id,RowVersion")] Cottage cottage)
        {
            if (id != cottage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cottageTmp = await _context.Cottage.FindAsync(id);
                    cottageTmp.Name = cottage.Name;
                    cottageTmp.Description = cottage.Description;
                    cottageTmp.RegularPrice = cottage.RegularPrice;
                    cottageTmp.WeekendPrice = cottage.WeekendPrice;
                    _context.Update(cottageTmp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageExists(cottage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("/Account/Manage/MyCottages", new { area = "Identity" });
            }
            return View(cottage);
        }
        
        // GET: Cottages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cottage == null)
            {
                return NotFound();
            }

            return View(cottage);
        }

        // POST: Cottages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cottage = await _context.Cottage.FindAsync(id);
            _context.Cottage.Remove(cottage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageExists(Guid id)
        {
            return _context.Cottage.Any(e => e.Id == id);
        }

    }
}
