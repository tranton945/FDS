using FDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FDS.Services
{
    public class Roles
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly FDSDbContext _context;

        public Roles(RoleManager<IdentityRole> roleManager, FDSDbContext context) 
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IdentityResult> AddRole(string roleName)
        {
            var role = new IdentityRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            return result;

        }

        public List<IdentityRole> GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return roles;
        }

        public IdentityRole GetByName(string role)
        {
            var _role = _roleManager.Roles.SingleOrDefault(r => r.Name == role);
            if (_role != null)
            {
                return _role;
            }
            return null;
        }

        public void DeleteRole(string role)
        {
            var _role = _roleManager.Roles.SingleOrDefault(r =>r.Name == role);
            if(role != null)
            {
                _roleManager.DeleteAsync(_role);
            }
        }

    }
}
