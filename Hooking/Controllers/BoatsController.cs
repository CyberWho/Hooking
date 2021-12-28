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
    public class BoatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BoatsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Boats
        public async Task<IActionResult> Index(string searchString = "",string sortOrder="")
        {
            List<Boat> boats = await _context.Boat.ToListAsync();
            ViewData["Name"] = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewData["Address"] = String.IsNullOrEmpty(sortOrder) ? "Address" : "";
            ViewData["City"] = String.IsNullOrEmpty(sortOrder) ? "City" : "";
            ViewData["Country"] = String.IsNullOrEmpty(sortOrder) ? "Country" : "";
            ViewData["AverageGrade"] = String.IsNullOrEmpty(sortOrder) ? "AverageGrade" : "";
            var bts = from b in _context.Boat
                      select b;
            switch (sortOrder)
            {
                case "Name":
                    bts = bts.OrderBy(b => b.Name);
                    break;
                case "Address":
                    bts = bts.OrderBy(b => b.Address);
                    break;
                case "City":
                    bts = bts.OrderBy(b => b.City);
                    break;
                case "Country":
                    bts = bts.OrderBy(b => b.Country);
                    break;
                case "AverageGrade":
                    bts = bts.OrderBy(b => b.AverageGrade);
                    break;

                default:
                    bts = bts.OrderByDescending(b => b.Name);
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bts = bts.Where(s => s.Name.Contains(searchString)
                                       || s.City.Contains(searchString) || s.Country.Contains(searchString));
            }
            return View(bts.ToList());
        }
        //GET : BoatOwner/Details/5/ShowMyBoats
        public async Task<IActionResult> ShowMyBoats()
        {
            var user = await _userManager.GetUserAsync(User);
            var boats = await _context.Boat.Where(m => m.BoatOwnerId == user.Id).ToListAsync();
            return View(boats);
        }

        // GET: Boats/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }
        [HttpGet("/Boats/MyBoatDetails/{id}")]
        public async Task<IActionResult> MyBoatDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat
                .FirstOrDefaultAsync(m => m.Id == id);
            var boatId = boat.Id.ToString();
            var boatOwnerUser = _context.UserDetails.Where(m => m.IdentityUserId == boat.BoatOwnerId).FirstOrDefault<UserDetails>();
            var fullAddress = boat.Address + "," + boat.City + "," + boat.Country;
            Guid boatCancelationPolicyId = Guid.Parse(boat.CancelationPolicyId);
            CancelationPolicy cancelationPolicy = _context.CancelationPolicy.Where(m => m.Id == boatCancelationPolicyId).FirstOrDefault<CancelationPolicy>();
            BoatFishingEquipment boatFishingEquipment = _context.BoatFishingEquipment.Where(m => m.BoatId == boatId).FirstOrDefault<BoatFishingEquipment>();
            Guid fishingEquipmentId = Guid.Parse(boatFishingEquipment.FishingEquipment);
            FishingEquipment fishingEquipment = _context.FishingEquipment.Where(m => m.Id == fishingEquipmentId).FirstOrDefault<FishingEquipment>();
            if (boat == null)
            {
                return NotFound();
            }
            ViewData["BoatOwner"] = boatOwnerUser;
            ViewData["FullAddress"] = fullAddress;
            ViewData["CancelationPolicy"] = cancelationPolicy;
            ViewData["FishingEquipment"] = fishingEquipment;
            return View(boat);
        }


        // GET: Boats/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult BoatSpecialOffers()
        {
            return Redirect("/BoatSpecialOffers");
        }

        // POST: Boats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type,Length,Capacity,EngineNumber,EnginePower,MaxSpeed,Address,City,Country,CancellationPolicyId,Description,AverageGrade,GradeCount,RegularPrice,WeekendPrice,BoatOwnerId,Id,RowVersion")] Boat boat)
        {
            if (ModelState.IsValid)
            {
                boat.Id = Guid.NewGuid();
                var user = await _userManager.GetUserAsync(User);
                boat.BoatOwnerId = user.Id;
                boat.CancelationPolicyId = "0";
                boat.AverageGrade = 0;
                boat.GradeCount = 0;
                _context.Add(boat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "BoatRules", new { id = boat.Id });
            }
            return View(boat);
        }

        // GET: Boats/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat.FindAsync(id);

            if (boat == null)
            {
                return NotFound();
            }
            return View(boat);
        }

        // POST: Boats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Address,City,Country,Description,RegularPrice,WeekendPrice,Id,RowVersion")] Boat boat)
        {
            if (id != boat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var boatTmp = await _context.Boat.FindAsync(id);
                    boatTmp.Name = boat.Name;
                    boatTmp.Address = boat.Address;
                    boatTmp.City = boat.City;
                    boatTmp.Country = boat.Country;
                    boatTmp.Description = boat.Description;
                    boatTmp.RegularPrice = boat.RegularPrice;
                    boatTmp.WeekendPrice = boat.WeekendPrice;
                    _context.Update(boatTmp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatExists(boat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("/Account/Manage/MyBoats", new { area = "Identity" });
            }
            return View(boat);
        }

        // GET: Boats/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boat = await _context.Boat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boat == null)
            {
                return NotFound();
            }

            return View(boat);
        }

        // POST: Boats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var boat = await _context.Boat.FindAsync(id);
            _context.Boat.Remove(boat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatExists(Guid id)
        {
            return _context.Boat.Any(e => e.Id == id);
        }
    }
}
