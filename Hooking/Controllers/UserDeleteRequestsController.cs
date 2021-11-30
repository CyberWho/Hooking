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
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using Newtonsoft.Json;

namespace Hooking.Controllers
{
    public class UserDeleteRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        [TempData]
        public string StatusMessage { get; set; }
        public UserDeleteRequestsController(ApplicationDbContext context,
                                            UserManager<IdentityUser> userManager,
                                            RoleManager<IdentityRole> roleManager,
                                            SignInManager<IdentityUser> signInManager,
                                            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            using (StreamReader reader = new StreamReader("./Data/emailCredentials.json"))
            {
                string json = reader.ReadToEnd();
                _emailSender = JsonConvert.DeserializeObject<EmailSender>(json);
            }
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
        public async Task<IActionResult> Create([Bind("Description,Id,RowVersion")] UserDeleteRequest userDeleteRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                List<UserDeleteRequest> userDeleteRequests = _context.UserDeleteRequest.Where(m => m.UserDetailsId == user.Id).ToList();
                foreach(var userDeleteRequestTemp in userDeleteRequests)
                {
                    if(!userDeleteRequestTemp.isReviewed)
                    {
                        StatusMessage = "Ne možete poslati više od jednog zahteva za brisanje profila.";
                        return RedirectToPage("/Account/Manage/UserDeleteRequest", new { area = "Identity" });
                    }
                }
                userDeleteRequest.Id = Guid.NewGuid();
                userDeleteRequest.IsApproved = false;
                userDeleteRequest.UserDetailsId = user.Id;
                userDeleteRequest.isReviewed = false;
                IList<string> rolenames = await _signInManager.UserManager.GetRolesAsync(user);
                switch (rolenames[0])
                {
                    case "Vlasnik vikendice":
                        userDeleteRequest.Type = DeletionType.COTTAGEOWNER;
                        break;
                    case "Korisnik":
                        userDeleteRequest.Type = DeletionType.USER;
                        break;
                    case "Instruktor":
                        userDeleteRequest.Type = DeletionType.INSTRUCTOR;
                        break;
                    case "Vlasnik broda":
                        userDeleteRequest.Type = DeletionType.BOATOWNER;
                        break;
                    case "Admin":
                        userDeleteRequest.Type = DeletionType.ADMIN;
                        break;
                }
                
                _context.Add(userDeleteRequest);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine(user.Email);
                await _emailSender.SendEmailAsync(user.Email, "Potvrda poslatog zahteva za brisanje profila",
                            $"Poštovani, Obaveštavamo Vas da smo primili Vaš zahtev za brisanje naloga. U narednom periodu ćete biti obavešteni o odluci admin tima. Hvala na strpljenju!");

                return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
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
