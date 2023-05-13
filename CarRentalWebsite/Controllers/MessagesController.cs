using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRentalWebsite.Controllers
{
    [Authorize(Roles = "Admin, Client")]
    public class MessagesController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
