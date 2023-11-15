using FDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FDS.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FDS.Services
{
    public class ManageUserRole
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FDSDbContext _context;

        public ManageUserRole(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, FDSDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> AddUserRole(string email, string roleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var role = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityResult> RemoveUserRole(string email, string roleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var role = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (user != null && role != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityResult> ChangeUserRole(string email, string newRoleName, string oldRoleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            var newRole = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == newRoleName);
            var oldRole = await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == oldRoleName);

            if (user != null && newRole != null)
            {
                var resultRemove = await _userManager.RemoveFromRoleAsync(user, oldRole.Name);

                if (resultRemove.Succeeded)
                {
                    var resultAdd = await _userManager.AddToRoleAsync(user, newRole.Name);
                    if (resultAdd.Succeeded)
                    {
                        return resultAdd;
                    }
                    else
                    {
                        return null;
                    }                    
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
