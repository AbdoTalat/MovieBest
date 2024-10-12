using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieBest.DAL.DbContext;
using MovieBest.DAL.Entities;
using Newtonsoft.Json;

namespace MovieBest.DAL.SeedData
{
    public static class AppSeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<MovieBestContext>();
            using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            Console.WriteLine("roles seed ...");

            // Seed Roles
            var roles = await ReadFromFile<List<string>>("Roles.json");
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            Console.WriteLine("Admin seed ...");
            // Seed Admin User
            var users = await ReadFromFile<List<UserModel>>("AppAdmin.json");

            foreach (var user in users)
            {
                if (!await userManager.Users.AnyAsync(u => u.UserName == user.UserName))
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                    };
                    var result = await userManager.CreateAsync(adminUser, user.Password);
                    if (result.Succeeded)
                    {
                        var rolesToAsign = await roleManager.Roles.ToListAsync();
                        if (rolesToAsign.Any(r => r.Name == "Admin"))
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                    }

                }
            }
        }
        private static async Task<T> ReadFromFile<T>(string path)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/SeedData", path);
            var jsonData = await File.ReadAllTextAsync(jsonFilePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

    }
}
public class UserModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

