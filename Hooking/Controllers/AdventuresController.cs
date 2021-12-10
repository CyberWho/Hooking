using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Models.DTO;
using Hooking.Services;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.AspNetCore.Identity;

namespace Hooking.Controllers
{
    public class AdventuresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdventureService _adventureService;
        private readonly UserManager<IdentityUser> _userManager;
       
        public AdventuresController(ApplicationDbContext context, 
            IAdventureService adventureService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _adventureService = adventureService;
            _userManager = userManager;
        }

        // GET: Adventures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Adventure.ToListAsync());
        }

        public IActionResult InstructorIndex()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adventures = _adventureService.GetInstructorAdventures(userId);

            return View(adventures);
        }
        public IActionResult MyAdventures()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adventures = _adventureService.GetInstructorAdventures(userId);

            return View(adventures);
        }
        [HttpGet("/Adventures/ShowAllUsers/{id}")]
        public async Task<IActionResult> ShowAllUsers(Guid id)
        {
            var adventureId = id;
            ViewData["AdventureId"] = adventureId;
            var allUsers = await _context.UserDetails.ToListAsync();
            
            return View(allUsers);
        }

        // GET: Adventures/Details/5
        public IActionResult Details(Guid? adventureId)
        {
            if (adventureId == null)
            {
                return NotFound();
            }

            AdventureDTO dto = _adventureService.GetAdventureDto((Guid)adventureId);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // GET: Adventures/Create
        public IActionResult Create(string instructorId)
        {
            Adventure newAdventure = new Adventure
            {
                InstructorId = instructorId
            };
            return View(newAdventure);
        }

        // POST: Adventures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,Address,City,Country,Description,MaxPersonCount,CancellationPolicyId,AverageGrade,Price,Id,RowVersion")] Adventure adventure)
        {
            if (ModelState.IsValid)
            {
                adventure.Id = Guid.NewGuid();
                _context.Add(adventure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(InstructorIndex));
            }
            return View(adventure);
        }

        // GET: Adventures/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventure = await _context.Adventure.FindAsync(id);
            if (adventure == null)
            {
                return NotFound();
            }
            return View(adventure);
        }

        // POST: Adventures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InstructorId,Name,Address,City,Country,Description,MaxPersonCount,CancellationPolicyId,AverageGrade,Price,Id,RowVersion")] Adventure adventure)
        {
            if (id != adventure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Adventure updatedAdventure = await _context.Adventure.FindAsync(adventure.Id);
                    _context.Entry(updatedAdventure).CurrentValues.SetValues(adventure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdventureExists(adventure.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(InstructorIndex));
            }
            return View(adventure);
        }
        public async Task<IActionResult> AdventureForSpecialOffer()
        {
            var user = await _userManager.GetUserAsync(User);

            Guid userId = Guid.Parse(user.Id);
            System.Diagnostics.Debug.WriteLine(userId);
            UserDetails userDetails = await _context.UserDetails.Where(m => m.IdentityUserId == user.Id).FirstOrDefaultAsync<UserDetails>();
            var userDetailsId = userDetails.Id.ToString();
            Instructor instructor = await _context.Instructor.Where(m => m.UserDetailsId == userDetailsId).FirstOrDefaultAsync<Instructor>();
            string instructorId = instructor.Id.ToString();
            List<Adventure> adventures = await _context.Adventure.Where(m => m.InstructorId == instructorId).ToListAsync<Adventure>();
            return View(adventures);
        }
        // GET: Adventures/Delete/5
        public async Task<IActionResult> Delete(Guid? adventureId, Guid? instructorId)
        {
            if (adventureId == null)
            {
                return NotFound();
            }

            var adventure = await _context.Adventure
                .FirstOrDefaultAsync(m => m.Id == adventureId);
            if (adventure == null)
            {
                return NotFound();
            }

            return View(adventure);
        }

        // POST: Adventures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var adventure = await _context.Adventure.FindAsync(id);
            _context.Adventure.Remove(adventure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(InstructorIndex));
        }

        private bool AdventureExists(Guid id)
        {
            return _context.Adventure.Any(e => e.Id == id);
        }
    }
}
