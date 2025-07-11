using AuthTrainingDL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthTrainingDL
{
    public class DataSeed
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeed(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedDataAsync()
        {
            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in Role.AllRoles)
                {
                    var resultCreateRole = await roleManager.CreateAsync(new IdentityRole() { Name = role });

                    if (!resultCreateRole.Succeeded)
                        throw new ArgumentException($"Don't append the {nameof(Role)}");
                }
            }

            if (!await userManager.Users.AnyAsync())
            {
                var administrator = new IdentityUser() { Email = "admin@mail.ru", UserName = "admin" };

                var resultCreateUser = await userManager.CreateAsync(administrator, "admin");

                if (!resultCreateUser.Succeeded)
                    throw new ArgumentException($"Don't append the {nameof(IdentityUser)}");

                var resultAddRole = await userManager.AddToRoleAsync(administrator, Role.Admin);

                if (!resultAddRole.Succeeded)
                    throw new ArgumentException($"Don't append the {nameof(Role)} to user");
            }
        }
    }
}
