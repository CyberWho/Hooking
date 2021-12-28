using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hooking.Data;
using Hooking.Models;
using Hooking.Services;

namespace Hooking.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFacilitiesService _facilitiesService;
        private readonly ICottageService _cottageService;
        public FacilitiesController(ApplicationDbContext context,
                                    IFacilitiesService facilitiesService,
                                    ICottageService cottageService)
        {
            _context = context;
            _facilitiesService = facilitiesService;
            _cottageService = cottageService;
        }

        // GET: Facilities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Facilities.ToListAsync());
        }

        // GET: Facilities/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilities = _facilitiesService.GetById(id);
            if (facilities == null)
            {
                return NotFound();
            }

            return View(facilities);
        }

        // GET: Facilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Facilities/Create/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id,[Bind("Parking,Wifi,Heating,BarbecueFacilities,OnlineCheckin,Jacuzzi,SeaView,MountainView,Kitchen,WashingMachine,AirportShuttle,IndoorPool,OutdoorPool,StockedBar,Garden,Id,RowVersion")] Facilities facilities)
        {
            if (ModelState.IsValid)
            {
                facilities.Id = Guid.NewGuid();
                _facilitiesService.Create(id, facilities);
                return RedirectToAction("AddRooms" , "CottageRooms" , new { id = id });
            }
            return View(facilities);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities == null)
            {
                return NotFound();
            }
            return View(facilities);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Facilities/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Parking,Wifi,Heating,BarbecueFacilities,OnlineCheckin,Jacuzzi,SeaView,MountainView,Kitchen,WashingMachine,AirportShuttle,IndoorPool,OutdoorPool,StockedBar,Garden,Id,RowVersion")] Facilities facilities)
        {
            if (id != facilities.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _facilitiesService.Update(id, facilities);
                    CottagesFacilities cottagesFacilities = _facilitiesService.GetByFacilitiesId(id);
                    return RedirectToAction("MyCottage", "Cottages", new { id = cottagesFacilities.CottageId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilitiesExists(facilities.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
            }
            return View(facilities);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilities = _facilitiesService.GetById(id);
            if (facilities == null)
            {
                return NotFound();
            }

            return View(facilities);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _facilitiesService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool FacilitiesExists(Guid id)
        {
            return _context.Facilities.Any(e => e.Id == id);
        }
    }
}
