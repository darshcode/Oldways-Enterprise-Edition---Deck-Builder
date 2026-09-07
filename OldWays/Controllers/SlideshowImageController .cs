using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create(int slideshowId)
        {
            return View(new SlideshowImage
            {
                SlideshowId = slideshowId,
                IsActive = true,
                DisplayOrder = 0
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SlideshowImage image, IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(upload.FileName)}";

                var container = _blob.GetBlobContainerClient("slideshow-images");
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                var blobClient = container.GetBlobClient(fileName);

                using (var stream = upload.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                image.ImageUrl = blobClient.Uri.ToString();
            }

            _db.SlideshowImages.Add(image);
            await _db.SaveChangesAsync();

            return RedirectToAction("Edit", "Slideshow", new { id = image.SlideshowId });
        }

        //(get) Edit action to display the edit form for a specific slideshow image
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var image = _db.SlideshowImages.FirstOrDefault(i => i.Id == id);

            if (image == null)
                return NotFound();

            return View(image);
        }

        //(post) Edit action to handle the form submission for editing a slideshow image
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(SlideshowImage image, IFormFile? upload)
        {
            var existing = _db.SlideshowImages.FirstOrDefault(i => i.Id == image.Id);

            if (existing == null)
                return NotFound();

            // If new file uploaded, replace blob
            if (upload != null && upload.Length > 0)
            {
                var container = _blob.GetBlobContainerClient("slideshow-images");
                await container.CreateIfNotExistsAsync();

                // Delete old blob
                if (!string.IsNullOrWhiteSpace(existing.ImageUrl))
                {
                    var oldBlobName = Path.GetFileName(existing.ImageUrl);
                    await container.GetBlobClient(oldBlobName).DeleteIfExistsAsync();
                }

                // Upload new blob
                var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(upload.FileName)}";
                var blobClient = container.GetBlobClient(newFileName);

                using (var stream = upload.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                existing.ImageUrl = blobClient.Uri.ToString();
            }

            // Update metadata
            existing.Title = image.Title;
            existing.AltText = image.AltText;
            existing.DisplayOrder = image.DisplayOrder;
            existing.IsActive = image.IsActive;

            await _db.SaveChangesAsync();

            return RedirectToAction("Edit", "Slideshow", new { id = existing.SlideshowId });
        }

        //(get) Delete action to display the delete confirmation page for a specific slideshow image
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var image = _db.SlideshowImages.FirstOrDefault(i => i.Id == id);

            if (image == null)
                return NotFound();

            return View(image);
        }

        [Authorize(Roles = "Admin")]
        // (post) DeleteConfirmed action to handle the deletion of a slideshow image
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = _db.SlideshowImages.FirstOrDefault(i => i.Id == id);

            if (image == null)
                return NotFound();

            var container = _blob.GetBlobContainerClient("slideshow-images");

            if (!string.IsNullOrWhiteSpace(image.ImageUrl))
            {
                var blobName = Path.GetFileName(image.ImageUrl);
                await container.GetBlobClient(blobName).DeleteIfExistsAsync();
            }

            _db.SlideshowImages.Remove(image);
            await _db.SaveChangesAsync();

            return RedirectToAction("Edit", "Slideshow", new { id = image.SlideshowId });
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
