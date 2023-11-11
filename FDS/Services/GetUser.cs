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

        public GetUser(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) 
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _email);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
