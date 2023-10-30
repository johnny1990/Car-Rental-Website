using CarRentalWebsite.Data;
using CarRentalWebsite.Data.Migrations;
using CarRentalWebsite.Database;
using CarRentalWebsite.Entities;
using CarRentalWebsite.Models;
using CarRentalWebsite.Services;
using CarRentalWebsite.SmtpService;
using IronBarCode;
using Microsoft.AspNetCore.Authorization;
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

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment,
            DBContext context, ApplicationDbContext dcContext, Smtp smtpService, 
            IOptions<ContactOptions> contactOptions)
        {
            _logger = logger;
            _environment = environment;
            _context = context;
            _dcContext = dcContext;
			_smtpService = smtpService;
			_contactOptions = contactOptions.Value;
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
            ////var UserName = User.Identity.Name.ToString();
            ////var Address = User.Identity..Address.ToString();
            ////var Mobile = User.Identity.Phone.ToString();
            ////var emergencyContact = User.Identity.EmergencyContact.ToString();
            ////var registrationDate = User.Identity.Date.ToString();
            //1.Name            --Vehicle 
            //2.Address         --?? auth
            //3.Owner mobile no  --?? auth
            //4.Emergency contact no  
            //5.Edit        -nok
            //
            // Delete        =nok
            // 6.Registration date 

            return View();
            //return _dcContext.Users != null ?
            //            View(await _dcContext.Users.ToListAsync()) :
            //            Problem("Entity set 'DBContext.Calls'  is null.");

        }


        [Authorize]
        public IActionResult GenerateQRCode()
        {
            var UserName = User.Identity.Name.ToString();

            ViewData["Vehicles"] = new SelectList(_context.Vehicles.Where(p => p.Owner == UserName && p.Validity != true), "Kit_Nr", "Kit_Nr");
            return View();
        }

        [HttpPost]
        public IActionResult GenerateQRCode(GenerateQRCodeModel generateQRCode, RegistrationQR qrmodel)
        {
            try
            {
                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(generateQRCode.QRCode, 200);
                barcode.AddBarcodeValueTextBelowBarcode();
                barcode.SetMargins(10);
                barcode.ChangeBarCodeColor(Color.Black);
                string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(_environment.WebRootPath, "GeneratedQRCode/qrcode.png");
                barcode.SaveAsPng(filePath);
                string fileName = Path.GetFileName(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                ViewBag.QrCodeUri = imageUrl;

                //save qr registration details 
                qrmodel.Owner = User.Identity.Name.ToString(); 
                qrmodel.Image = Encoding.ASCII.GetBytes(imageUrl);
                qrmodel.Generation_Date = DateTime.Now;

    
                _context.Add(qrmodel);
                _context.SaveChanges();


                //update GenerationDate&Validity fields 
                var entity = _context.Vehicles.FirstOrDefault(item => item.Id == qrmodel.Id);
                if (entity != null)
                {
                
                    entity.Activation_Date = qrmodel.Generation_Date;
                    entity.Validity = true;

                    _context.Vehicles.Update(entity);
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }

            var UserName = User.Identity.Name.ToString();

            ViewData["Vehicles"] = new SelectList(_context.Vehicles.Where(p => p.Owner == UserName), "Kit_Nr", "Kit_Nr");
            return View();
        }


        //Generate OTP
        #region GenerateOTP
       
        [HttpGet]
        public IActionResult GenerateOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendOTP()
        {
            string num = "0123456789";
            int len = num.Length;
            string otp = string.Empty;
            int otpdigit = 10;
            string finaldigit;
            int getIndex;

            for(int i = 0; i < otpdigit; i++ )
            {
                do
                {
                    getIndex = new Random().Next(0, len);
                    finaldigit = num.ToCharArray()[getIndex].ToString();
                }
                   while (otp.IndexOf(finaldigit) != -1);
                otp += finaldigit;
            }

            TempData["otp"] = otp;


           return RedirectToAction("GenerateOTP", "Home");
        }

        public IActionResult ValidateOTP()
        {
            return View();
        }
        #endregion

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