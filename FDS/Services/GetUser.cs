using FDS.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace FDS.Services
{
    public class GetUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FDSDbContext _context;

        public GetUser(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, FDSDbContext context) 
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<ApplicationUser> user()
        {
            // lấy jwt mà user cung cấp
            var authResult = _httpContextAccessor.HttpContext.AuthenticateAsync().Result;
            var token = authResult.Properties.GetTokenValue("access_token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            // lấy email từ chuỗi jwt sau khi giải mã jwt
            var _email = jwtToken.Claims.FirstOrDefault().Value;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == _email);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<List<Group>> userInGroup()
        {
            var _user = await user();
            if (_user == null)
            {
                return null;
            }
            var group = await _context.Groups
                                .Where(gr => gr.Members.Any(member => member.UserID == _user.Id))
                                .ToListAsync();
            return group;
        }

        public async Task<List<GroupType>> ListGroupsType()
        {
            var group = await userInGroup();
            if (group.Count() == 0)
            {
                return null;
            }
            // get list groupId 
            var groupIds = group.Select(group=> group.Id).ToList();

            // Tìm list GroupType có GroupId trùng với Id của list groupIds  
            var groupsType = await _context.GroupTypes
                                        .Where(gt => groupIds.Contains(gt.GroupId))
                                        .ToListAsync();
            return groupsType;
        }

    }
}
