
using Microsoft.AspNetCore.Mvc;
using OldWays.Models;

namespace OldWays.Controllers
{
    public class Contact : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
