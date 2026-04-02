using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OldWays.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OldWays.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BlobServiceClient _blobServiceClient;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BlobServiceClient blobServiceClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _blobServiceClient = blobServiceClient;
        }

        public string? ProfilePhotoUrl { get; set; }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

    
        public class InputModel
        {

            [Display(Name = "Profile Photo")]
            public IFormFile? ProfilePhoto { get; set; }


            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PostalCode = user.PostalCode,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            ProfilePhotoUrl = user.ProfilePictureUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                ProfilePhotoUrl = user.ProfilePictureUrl;
                return Page();
            }

            // photo upload
            if (Input.ProfilePhoto != null)
            {
                var file = Input.ProfilePhoto;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                //pass extension check
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Input.ProfilePhoto", "Only JPG and PNG files are allowed.");
                    await LoadAsync(user);
                    ProfilePhotoUrl = user.ProfilePictureUrl;
                    return Page();
                }

                // file size limit 5GB = 5 *1024^2
                if (file.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("Input.ProfilePhoto", "File size must be under 5MB.");
                    await LoadAsync(user);
                    ProfilePhotoUrl = user.ProfilePictureUrl;
                    return Page();
                }

                // Upload to Azure Blob Storage
                var container = _blobServiceClient.GetBlobContainerClient("profile-pictures");
                await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var fileName = $"{user.Id}.jpg";
                var blob = container.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blob.UploadAsync(stream, overwrite: true);
                }

                user.ProfilePictureUrl = blob.Uri.ToString();
            }


            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }


            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }


            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
