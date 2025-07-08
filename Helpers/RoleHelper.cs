using Messanger.Models;
using Microsoft.AspNetCore.Identity;

namespace Messanger.Helpers
{
    public static class RoleHelper
    {
        private static readonly string[] roles = { "Admin", "User" };

        public static async Task SeedRolesAsync(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var roleManager = scope.ServiceProvider
                                      .GetRequiredService<RoleManager<IdentityRole<int>>>();

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(role));
                    }
                }
            }
        }

        public static async Task SeedAdminAsync(this IServiceProvider services, IConfiguration config)
        {

            var email = config["Admin:Email"];
            var userName = config["Admin:UserName"];
            var password = config["Admin:Password"];

            using (var scope = services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var admin = await roleManager.FindByEmailAsync(email);

                if (admin is null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true
                    };

                    await roleManager.CreateAsync(admin, password);

                    if (!await roleManager.IsInRoleAsync(admin, "Admin"))
                    {
                        await roleManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }
}
