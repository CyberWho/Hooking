using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hooking.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Hooking.Controllers
{
    public class RegistrationRequestsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RegistrationRequestsController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: RegistrationRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistrationRequest.ToListAsync());
        }

        // GET: RegistrationRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationRequest = await _context.RegistrationRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrationRequest == null)
            {
                return NotFound();
            }

            return View(registrationRequest);
        }

        // GET: RegistrationRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistrationRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserDetailsId,Type,Description,Id,RowVersion")] RegistrationRequest registrationRequest)
        {
            if (ModelState.IsValid)
            {
                registrationRequest.Id = Guid.NewGuid();
                _context.Add(registrationRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registrationRequest);
        }

        // GET: RegistrationRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationRequest = await _context.RegistrationRequest.FindAsync(id);
            if (registrationRequest == null)
            {
                return NotFound();
            }
            return View(registrationRequest);
        }

        // POST: RegistrationRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserDetailsId,Type,Description,Id,RowVersion")] RegistrationRequest registrationRequest)
        {
            if (id != registrationRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationRequestExists(registrationRequest.Id))
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
            return View(registrationRequest);
        }

        // GET: RegistrationRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationRequest = await _context.RegistrationRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registrationRequest == null)
            {
                return NotFound();
            }

            return View(registrationRequest);
        }

        // POST: RegistrationRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var registrationRequest = await _context.RegistrationRequest.FindAsync(id);
            _context.RegistrationRequest.Remove(registrationRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private CottageOwner CreateCottageOwner(UserDetails userDetails)
        {
            CottageOwner cottageOwner = new CottageOwner
            {
                Id = Guid.NewGuid(), UserDetailsId = userDetails.Id.ToString(), AverageGrade = 0, GradeCount = 0
            };
            return cottageOwner;
        }

        private BoatOwner createBoatOwner(UserDetails userDetails)
        {
            BoatOwner boatOwner = new BoatOwner
            {
                Id = Guid.NewGuid(),
                UserDetailsId = userDetails.Id.ToString(),
                AverageGrade = 0,
                GradeCount = 0,
                IsCaptain = false,
                IsFirstOfficer = false
            };
            return boatOwner;
        }

        private Instructor CreateInstructor(UserDetails userDetails)
        {
            Instructor instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                UserDetailsId = userDetails.Id.ToString(),
                AverageGrade = 0,
                Biography = "",
                GradeCount = 0
            };
            return instructor;
        }

        public async Task<IActionResult> Approve(Guid id)
        {
            var request = await _context.RegistrationRequest.FindAsync(id);
            _context.RegistrationRequest.Remove(request);
            await _context.SaveChangesAsync();

            UserDetails userDetails = await _context.UserDetails.FindAsync(Guid.Parse(request.UserDetailsId));
            userDetails.Approved = true;

            switch (request.Type)
            {
                case RegistrationType.COTTAGE_OWNER:
                {
                    CottageOwner cottageOwner = CreateCottageOwner(userDetails);
                    _context.Add(cottageOwner);
                    await _context.SaveChangesAsync();
                    break;
                }
                case RegistrationType.BOAT_OWNER:
                {
                    BoatOwner boatOwner = createBoatOwner(userDetails);
                    _context.Add(boatOwner);
                    await _context.SaveChangesAsync();
                    break;
                }
                case RegistrationType.INSTRUCTOR:
                {
                    Instructor instructor = CreateInstructor(userDetails);
                    _context.Add(instructor);
                    await _context.SaveChangesAsync();
                    break;
                }
            }
            

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            var request = await _context.RegistrationRequest.FindAsync(id);
            _context.RegistrationRequest.Remove(request);
            await _context.SaveChangesAsync();

            UserDetails userDetails = await _context.UserDetails.FindAsync(Guid.Parse(request.UserDetailsId));
            _context.UserDetails.Remove(userDetails);
            IdentityUser user = await _userManager.FindByIdAsync(userDetails.IdentityUserId);
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationRequestExists(Guid id)
        {
            return _context.RegistrationRequest.Any(e => e.Id == id);
        }
    }
}
