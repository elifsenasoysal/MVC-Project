using Microsoft.AspNetCore.Identity;

namespace SporSalonuYonetimi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Rolleri Ekle
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // Admin Ekle
            var adminEmail = "b231210059@sakarya.edu.tr"; // Numaranı buraya yaz
            var adminPassword = "sau"; // Şifre: sau
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newAdmin, adminPassword);
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}