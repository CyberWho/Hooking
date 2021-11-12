using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hooking.Data;
using Hooking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hooking.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,            
            IEmailSender emailSender, 
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;

            using (StreamReader reader = new StreamReader("./Data/emailCredentials.json"))
            {
                string json = reader.ReadToEnd();
                _emailSender = JsonConvert.DeserializeObject<EmailSender>(json);
            }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Polje 'E-mail adresa' je obavezno")]
            [EmailAddress(ErrorMessage = "E-mail adresa nije u validnom formatu")]
            [Display(Name = "E-mail adresa")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Polje 'Lozinka' je obavezno")]
            [StringLength(100, ErrorMessage = "{0} mora biti dugačka bar {2} i najviše {1} karaktera.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrdite lozinku")]
            [Compare("Password", ErrorMessage = "Unete lozinke se ne poklapaju.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Polje 'Ime' je obavezno")]
            [Display(Name = "Ime")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Polje 'Prezime' je obavezno")]
            [Display(Name = "Prezime")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Polje 'Grad i država' je obavezno")]
            [Display(Name = "Grad i država")]
            public string Location { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var userDetails = new UserDetails();
                userDetails.FirstName = Input.Name;
                userDetails.LastName = Input.LastName;
                userDetails.City = Input.Location.Split(",")[0];
                userDetails.Country = Input.Location.Split(",")[1];
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    userDetails.IdentityUserId = user.Id;
                    var resultUserDetails = _context.Add(userDetails);

                    if (resultUserDetails != null)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Potvrdite Vašu e-mail adresu",
                            $"Poštovani,<br><br>molimo Vas da potvrdite Vašu registraciju na Hooking klikom na <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ovaj link</a>.");

                        var userCount = _userManager.Users.Count();
                        Console.WriteLine("Trenutni broj korisnika: " + userCount.ToString());
                        if (userCount == 1)
                        {
                            if (_roleManager.Roles.ToList().Count == 0)
                            {
                                IdentityRole role = new IdentityRole
                                {
                                    Name = "Admin"
                                };
                                await _roleManager.CreateAsync(role);
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                        }

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
