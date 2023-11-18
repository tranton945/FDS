using FDS.Data;
using FDS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace FDS.Services
{
    public class GroupRepository : IGroupRepository
    {
        private readonly FDSDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly GetUser _getUser;

        public GroupRepository(GetUser getUser, FDSDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _getUser = getUser;
        }

        public async Task<bool> checkAccountPermission()
        {
            var user = await _getUser.user();
            var adminRole = await _userManager.GetRolesAsync(user);
            if (adminRole.Any(a => a.ToUpper() == "ADMIN"))
            {
                return true;
            }
            var listAccountPermission = await _context.AccountPermissions.ToListAsync();
            if (listAccountPermission.Any(a => a.UsreName == user.UserName))
            {
                return true;
            }
            return false;
        }

        public async Task<Data.Group> Add(Data.Group g)
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var user = await _getUser.user();

            var group = new Data.Group
            {
                Name = g.Name,
                CreateDate = DateTime.UtcNow,
                Note = g.Note,
                Creator = user.UserName,
                Members = g.Members
            };
            await _context.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
            
        }
        // kiểm tra user đã có trong group hay chưa
        public async Task<List<string>> CheckUsersNotInGroup(List<string> usernames, int groupId)
        {
            var group = await _context.Groups.Include(a => a.Members).SingleOrDefaultAsync(u => u.Id == groupId);
            var listMember = group.Members;
            List<string> result = new List<string>();

            foreach (var username in usernames)
            {
                bool isUserInGroup = listMember.Any(member => member.UserName == username);
                if (!isUserInGroup && !result.Contains(username))
                {
                    result.Add(username);
                }
            }
            return result;
        }

        public async Task<Data.Group> AddMember(List<string> username, int groupId)
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var group = await _context.Groups.Include(a => a.Members).SingleOrDefaultAsync(u => u.Id == groupId);
            if (group == null)
            {
                return null;
            }
            if (group.Members == null)
            {
                group.Members = new List<UserDTO>();
            }
            var ListUsersNotInGroup = await CheckUsersNotInGroup(username, groupId);
            if (ListUsersNotInGroup == null)
            {
                return null;
            }
            foreach (var a in ListUsersNotInGroup)
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == a);
                if(user == null)
                {
                    return null;
                }

                var role = await _userManager.GetRolesAsync(user);
                var permissions = string.Join(",", role);

                var  newUser = new UserDTO
                {
                    UserID = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Permission = permissions,
                    GroupId = groupId
                };
                group.Members.Add(newUser);

            }
            await _context.SaveChangesAsync();

            return group;
        }

        public async Task<bool> Delete(int id)
        {
            if (await checkAccountPermission() == false)
            {
                return false;
            }
            var result = await _context.Groups
                            .Include(a => a.Members)
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (result == null)
            {
                return false;
            }
            // Xóa tất cả các member liên kết
            _context.UserDTOs.RemoveRange(result.Members);
            _context.Groups.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Data.Group>> GetAll()
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var result = await _context.Groups.Include(a => a.Members).ToListAsync();
            return result;
        }

        public async Task<Data.Group> GetById(int id)
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var result = await _context.Groups
                            .Include(a => a.Members)
                            .FirstOrDefaultAsync(f => f.Id == id);
            if (result == null)
            {
                return null;
            }
            return result;
        }
        public async Task<bool> DeleteMembers(List<string> listUsername, int groupId)
        {
            if (await checkAccountPermission() == false)
            {
                return false;
            }
            var result = await _context.Groups.Include(a => a.Members).SingleOrDefaultAsync(f => f.Id == groupId);
            if (result == null)
            {
                return false;
            }
            // Xác định members cần xóa
            var membersToRemove = result.Members
                .Where(member => listUsername.Contains(member.UserName))
                .ToList();
            foreach (var memberToRemove in membersToRemove)
            {
                result.Members.Remove(memberToRemove);
            }
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> Update(Data.Group g, int id)
        {
            if (await checkAccountPermission() == false)
            {
                return false;
            }
            var result = await _context.Groups.Include(a => a.Members).SingleOrDefaultAsync(f => f.Id == id);
            if (result == null)
            {
                return false;
            }
            result.Name = g.Name ?? result.Name;
            result.CreateDate = result.CreateDate;
            result.Note = g.Note ?? result.Note;
            result.Creator = result.Creator;

            _context.Update(result);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
