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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Hooking.Controllers
{
    public class AdventuresController : Controller
    {
        private readonly IAdventureService _adventureService;
        private readonly ApplicationDbContext _context;
        public List<AdventureImage> adventureImages = new List<AdventureImage>();

        public AdventuresController(IAdventureService adventureService, 
            ApplicationDbContext context)
        {
            _adventureService = adventureService;
            _context = context;
        }

        // GET: Adventures
        public IActionResult Index()
        {
            return View(_adventureService.GetAdventures());
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
        public IActionResult ShowAllUsers(Guid id)
        {
            var adventureId = id;
            ViewData["AdventureId"] = adventureId;

            var allUsers = _adventureService.GetAllUserDetails();
            
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
            adventureImages = _adventureService.GetAdventureImages(adventureId ?? throw new NullReferenceException()).ToList();
            ViewData["AdventureImages"] = adventureImages;
            return View(dto);
        }
        [HttpPost("/Adventures/UploadImage/{id}")]
        public async Task<ActionResult> UploadImage(Guid id, IFormFile file)
        {
            if (file != null)
            {
                string ContainerName = "adventure"; //hardcoded container name
                string fileName = Path.GetFileName(file.FileName);
                using (var fileStream = file.OpenReadStream())
                {
                    string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName=hookingstorage;AccountKey=+v8L5XkQZ7Z2CTfdTd03pngWlA4xu02caFJDGUkvGlo/rv8uZnM9CQQYleH3lpb+3Z8sefUOlC0EaoXWIquyDg==;EndpointSuffix=core.windows.net");
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(UserConnectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToLower());
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                    try
                    {
                        await blockBlob.UploadFromStreamAsync(fileStream);

                    }
                    catch (Exception e)
                    {
                        var r = e.Message;
                        return null;
                    }

                    if (blockBlob != null)
                    {
                        _adventureService.AddAdventureImage(id, blockBlob);
                    }

                }
                return RedirectToAction(nameof(InstructorIndex));
            }
            else
            {
                return RedirectToAction(nameof(InstructorIndex));
            }


        }

        // GET: Adventures/Create
        public IActionResult Create(string instructorId)
        {
            Adventure newAdventure = new Adventure
            {
                InstructorId = instructorId
            };
            return View(new AdventureDTO(newAdventure));
        }

        // POST: Adventures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("InstructorId,Name,Address,City,Country,Description,MaxPersonCount,CancellationPolicyId,ChildFriendly,YouKeepCatch,CatchAndReleaseAllowed,CabinSmoking,AverageGrade,Price")] AdventureDTO dto)
        {
            if (ModelState.IsValid)
            {
                _adventureService.Create(dto);

                return RedirectToAction(nameof(InstructorIndex));
            }
            return View(dto);
        }

        // GET: Adventures/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adventure = _adventureService.FindAdventureById(id ?? throw new NullReferenceException());
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
                    _adventureService.UpdateAdventure(adventure);
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
        public IActionResult AdventureForSpecialOffer()
        {
            List<Adventure> adventures = _adventureService.GetAdventuresForSpecialOffer(User).ToList();
            return View(adventures);
        }
        // GET: Adventures/Delete/5
        public async Task<IActionResult> Delete(Guid? adventureId, Guid? instructorId)
        {
            if (adventureId == null)
            {
                return NotFound();
            }

            var adventure = _adventureService.FindAdventureById(adventureId ?? throw new NullReferenceException());
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
            List<AdventureRealisation> adventureRealisations = new List<AdventureRealisation>();
            string adventureId = adventure.Id.ToString();
            adventureRealisations = await _context.AdventureRealisation.Where(m => m.AdventureId == adventureId).ToListAsync();
            if(adventureRealisations.Count == 0)
            {
                _context.Adventure.Remove(adventure);
            }
            List<AdventureSpecialOffer> adventureSpecialOffers = new List<AdventureSpecialOffer>();
            adventureSpecialOffers = await _context.AdventureSpecialOffer.Where(m => m.AdventureId == adventureId).ToListAsync();
            if(adventureSpecialOffers.Count == 0)
            {
                _context.Adventure.Remove(adventure);
                AdventuresAdventureRules rules = _context.AdventuresAdventureRules.FirstOrDefault(r => r.AdventureId == adventureId);
                _context.AdventuresAdventureRules.Remove(rules);
                await _context.SaveChangesAsync();
            }

           return RedirectToAction(nameof(InstructorIndex));
        }

        private bool AdventureExists(Guid id)
        {
            return _adventureService.AdventureExists(id);
        }
    }
}
