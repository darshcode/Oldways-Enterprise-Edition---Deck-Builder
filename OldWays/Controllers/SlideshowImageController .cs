using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using OldWays.Data;
using OldWays.Models;

namespace OldWays.Controllers
{



    public class SlideshowImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly BlobServiceClient _blob;

        public SlideshowImageController(ApplicationDbContext db, BlobServiceClient blob)
        {
            _db = db;
            _blob = blob;
        }


        public IActionResult Create(int slideshowId)
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(SlideshowImage image, IFormFile upload)
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
