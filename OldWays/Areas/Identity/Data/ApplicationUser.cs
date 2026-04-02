
using Microsoft.AspNetCore.Identity;

namespace OldWays.Areas.Identity.Data
{
    public class ApplicationUser:IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public string? PostalCode { get; set; }

        [PersonalData]
        public string? ProfilePictureUrl { get; set; }
    }
}
