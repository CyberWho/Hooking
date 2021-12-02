using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;

namespace Hooking.Controllers
{
    public class RegistrationRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationRequestsController(ApplicationDbContext context)
        {
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

        private bool RegistrationRequestExists(Guid id)
        {
            return _context.RegistrationRequest.Any(e => e.Id == id);
        }
    }
}
