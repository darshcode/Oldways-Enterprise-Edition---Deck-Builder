using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OldWays.Data;
using OldWays.Models;
using SQLitePCL;

namespace OldWays.Controllers
{
    public class SlideshowController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly BlobServiceClient _blob;

        public SlideshowController(ApplicationDbContext context, BlobServiceClient blobServiceClient)
        {
            _db = context;
            _blob = blobServiceClient;
        }

        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Slideshow slideshow)
        {
            _db.Slideshows.Add(slideshow);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var slideshow = _db.Slideshows
                .Include(s => s.Images)
                .FirstOrDefault(s => s.Id == id);

            if (slideshow == null)
                return NotFound();

            return View(slideshow);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Slideshow slideshow)
        {
            if (!ModelState.IsValid)
                return View(slideshow);

            _db.Slideshows.Update(slideshow);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var slideshow = _db.Slideshows.Include(s => s.Images)
        .FirstOrDefault(s => s.Id == id);

            if (slideshow == null)
                return NotFound();

            return View(slideshow);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var slideshow = _db.Slideshows.Include(s => s.Images)
        .FirstOrDefault(s => s.Id == id);

            if (slideshow == null)
                return NotFound();

            // Remove images first like cascade delete, then remove the slideshow.
            _db.SlideshowImages.RemoveRange(slideshow.Images);
            _db.Slideshows.Remove(slideshow);

            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }

}
