using CarRentalWebsite.Database;
using CarRentalWebsite.Entities;
using CarRentalWebsite.Models;
using IronBarCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace CarRentalWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly DBContext _context;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, DBContext context)
        {
            _logger = logger;
            _environment = environment;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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



        #endregion


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}