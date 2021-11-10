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
    public class UserDeleteRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserDeleteRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserDeleteRequests
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserDeleteRequest.ToListAsync());
        }

        // GET: UserDeleteRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDeleteRequest = await _context.UserDeleteRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDeleteRequest == null)
            {
                return NotFound();
            }

            return View(userDeleteRequest);
        }

        // GET: UserDeleteRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDeleteRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserDetailsId,Description,IsApproved,Type,Id,RowVersion")] UserDeleteRequest userDeleteRequest)
        {
            if (ModelState.IsValid)
            {
                userDeleteRequest.Id = Guid.NewGuid();
                _context.Add(userDeleteRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userDeleteRequest);
        }

        // GET: UserDeleteRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDeleteRequest = await _context.UserDeleteRequest.FindAsync(id);
            if (userDeleteRequest == null)
            {
                return NotFound();
            }
            return View(userDeleteRequest);
        }

        // POST: UserDeleteRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserDetailsId,Description,IsApproved,Type,Id,RowVersion")] UserDeleteRequest userDeleteRequest)
        {
            if (id != userDeleteRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDeleteRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDeleteRequestExists(userDeleteRequest.Id))
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
            return View(userDeleteRequest);
        }

        // GET: UserDeleteRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDeleteRequest = await _context.UserDeleteRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDeleteRequest == null)
            {
                return NotFound();
            }

            return View(userDeleteRequest);
        }

        // POST: UserDeleteRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userDeleteRequest = await _context.UserDeleteRequest.FindAsync(id);
            _context.UserDeleteRequest.Remove(userDeleteRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDeleteRequestExists(Guid id)
        {
            return _context.UserDeleteRequest.Any(e => e.Id == id);
        }
    }
}
