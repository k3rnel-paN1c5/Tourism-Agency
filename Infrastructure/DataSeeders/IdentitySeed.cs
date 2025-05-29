using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infrastructure.DataSeeders;

/// <summary>
/// Provides static methods for seeding identity-related data into the database.
/// </summary>
public static class IdentitySeed
{

    /// <summary>
    /// Seeds predefined roles and an initial administrator user into the identity system.
    /// </summary>
    /// <param name="userManager">The UserManager instance for managing users.</param>
    /// <param name="roleManager">The RoleManager instance for managing roles.</param>
    /// <returns>A task that represents the asynchronous seeding operation.</returns>
    public static async Task SeedRolesAndAdmin(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Manager", "TripSupervisor", "BookingSupervisor", "Customer" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@gmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
            };

            var result = await userManager.CreateAsync(user, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

    }
}
