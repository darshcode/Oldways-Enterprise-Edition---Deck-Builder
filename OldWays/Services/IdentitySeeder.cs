using Microsoft.AspNetCore.Identity;

namespace OldWays.Services
{
    public class IdentitySeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            Console.WriteLine(">>> Running Role Seeder...");

        }

    }
}
