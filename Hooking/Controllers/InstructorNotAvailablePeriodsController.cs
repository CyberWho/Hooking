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
    public class InstructorNotAvailablePeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public Instructor instructor;
        public InstructorNotAvailablePeriodsController(ApplicationDbContext context,
                                                        SignInManager<IdentityUser> signInManager,
                                                        UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            Guid userId = Guid.Parse(user.Id);
            System.Diagnostics.Debug.WriteLine(userId);
            UserDetails userDetails = await _context.UserDetails.Where(m => m.IdentityUserId == user.Id).FirstOrDefaultAsync<UserDetails>();
            var userDetailsId = userDetails.Id.ToString();
            instructor = await _context.Instructor.Where(m => m.UserDetailsId == userDetailsId).FirstOrDefaultAsync<Instructor>();
            string instructorId = instructor.Id.ToString();
            List<Adventure> adventures = await _context.Adventure.Where(m => m.InstructorId == instructorId).ToListAsync<Adventure>();
            List<AdventureRealisation> adventureRealisations = new List<AdventureRealisation>();
            foreach(Adventure adventure in adventures)
            {
                var adventureId = adventure.Id.ToString();
                List<AdventureRealisation> adventureRealisationsTemp = await _context.AdventureRealisation.Where(m => m.AdventureId == adventureId).ToListAsync<AdventureRealisation>();
                adventureRealisations.AddRange(adventureRealisationsTemp);
            }
            string codeForFront = "[";

            int i = 0;
            string title = "Rezervacija";
            foreach (var adventureRealization in adventureRealisations)
            {
                if (i++ > 0)
                {
                    codeForFront += ",";
                }

                
                DateTime end = adventureRealization.StartDate.AddMinutes(adventureRealization.Duration);
                codeForFront += "{ title: '" + title + "', start: '" +
                    adventureRealization.StartDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "', " +
                    "end: '" + end.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss") + "'}\n";
            }
            codeForFront += "]";
            codeForFront = codeForFront.Replace("‘", "").Replace("’", "");
            ViewData["codeForFront"] = codeForFront;
            ViewData["Instructor"] = instructor;
            return View();
        }

        // GET: InstructorNotAvailablePeriods/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorNotAvailablePeriod = await _context.InstructorNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructorNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(instructorNotAvailablePeriod);
        }

        // GET: InstructorNotAvailablePeriods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InstructorNotAvailablePeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartTime,EndTime,Id,RowVersion")] InstructorNotAvailablePeriod instructorNotAvailablePeriod)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                Guid userId = Guid.Parse(user.Id);
                System.Diagnostics.Debug.WriteLine(userId);
                UserDetails userDetails = await _context.UserDetails.Where(m => m.IdentityUserId == user.Id).FirstOrDefaultAsync<UserDetails>();
                var userDetailsId = userDetails.Id.ToString();
                instructor = await _context.Instructor.Where(m => m.UserDetailsId == userDetailsId).FirstOrDefaultAsync<Instructor>();
                string instructorId = instructor.Id.ToString();
                instructorNotAvailablePeriod.Id = Guid.NewGuid();
                instructorNotAvailablePeriod.InstructorId = instructorId;
                _context.Add(instructorNotAvailablePeriod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructorNotAvailablePeriod);
        }

        // GET: InstructorNotAvailablePeriods/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorNotAvailablePeriod = await _context.InstructorNotAvailablePeriod.FindAsync(id);
            if (instructorNotAvailablePeriod == null)
            {
                return NotFound();
            }
            return View(instructorNotAvailablePeriod);
        }

        // POST: InstructorNotAvailablePeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InstructorId,StartTime,EndTime,Id,RowVersion")] InstructorNotAvailablePeriod instructorNotAvailablePeriod)
        {
            if (id != instructorNotAvailablePeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructorNotAvailablePeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorNotAvailablePeriodExists(instructorNotAvailablePeriod.Id))
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
            return View(instructorNotAvailablePeriod);
        }

        // GET: InstructorNotAvailablePeriods/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorNotAvailablePeriod = await _context.InstructorNotAvailablePeriod
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructorNotAvailablePeriod == null)
            {
                return NotFound();
            }

            return View(instructorNotAvailablePeriod);
        }

        // POST: InstructorNotAvailablePeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var instructorNotAvailablePeriod = await _context.InstructorNotAvailablePeriod.FindAsync(id);
            _context.InstructorNotAvailablePeriod.Remove(instructorNotAvailablePeriod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorNotAvailablePeriodExists(Guid id)
        {
            return _context.InstructorNotAvailablePeriod.Any(e => e.Id == id);
        }
    }
}
