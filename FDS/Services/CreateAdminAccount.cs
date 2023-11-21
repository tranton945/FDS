using FDS.Data;
using Microsoft.AspNetCore.Identity;

namespace FDS.Services
{
    public class CreateAdminAccount
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateAdminAccount(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }


        public async Task Start()
        {
            await CreateRolesAsync();
            await CreateAdminUserAsync();
        }

        private async Task CreateAdminUserAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("systemadmin123@vietjetair.com");
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    Name = "systemadmin123",
                    Email = "systemadmin123@vietjetair.com",
                    DateOfBirt = DateTime.Now,
                    Gender = "male",
                    UserName = "systemadmin123@vietjetair.com",
                };

                var createAdminAccount = await _userManager.CreateAsync(admin, "AdminVietjetair@123");

                if (createAdminAccount.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }

        }

        private async Task CreateRolesAsync()
        {
            var roleExist = await _roleManager.RoleExistsAsync("Admin");
            if(!roleExist)
            {
                var role = new IdentityRole { Name = "Admin" };
                var result = await _roleManager.CreateAsync(role);
            }
        }
    }
}
