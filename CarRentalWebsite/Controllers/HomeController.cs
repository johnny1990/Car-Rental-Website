using CarRentalWebsite.Data;
using CarRentalWebsite.Data.Migrations;
using CarRentalWebsite.Database;
using CarRentalWebsite.Entities;
using CarRentalWebsite.Models;
using CarRentalWebsite.Services;
using CarRentalWebsite.SmtpService;
using IronBarCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRentalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly DBContext _context;
        private readonly ApplicationDbContext _dcContext;
        private readonly Smtp _smtpService;
        readonly ContactOptions _contactOptions;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment,
            DBContext context, ApplicationDbContext dcContext, Smtp smtpService, 
            IOptions<ContactOptions> contactOptions, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _environment = environment;
            _context = context;
            _dcContext = dcContext;
			_smtpService = smtpService;
			_contactOptions = contactOptions.Value;
            _userManager = userManager;
		}


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(Contact form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var htmlBody = $"<p>{form.Name} ({form.Email})</p><p>{form.Phone}</p><p>{form.Message}</p>";
            var textBody = "{formData.Name} ({formData.Email})\r\n{formData.Phone}\r\n{formData.Message}";

            _smtpService.SendSingle("Contact Form", htmlBody, textBody,
                                    _contactOptions.ContactToName, _contactOptions.ContactToAddress,
                                    _contactOptions.ContactFromName, _contactOptions.ContactFromAddress);

            TempData["Message"] = "Thank you! Your message is sent to us.";

		    return RedirectToAction("Contact");
        }

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> OwnerDetails()
		{
			return View(await _userManager.Users.ToListAsync());
		}

        [HttpGet]
        public IActionResult RegisterInfo()
        {
            return View();
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }  
        
         public IActionResult Privacy()
        {
            return View();
        }

    }
}