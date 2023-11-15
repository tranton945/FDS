using FDS.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace FDS.Services
{
    public class BlacklistService
    {
        private readonly FDSDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlacklistService(FDSDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckJWT()
        {
            // lấy jwt mà user cung cấp
            var authResult = _httpContextAccessor.HttpContext.AuthenticateAsync().Result;
            var token = authResult.Properties.GetTokenValue("access_token");

            if (token == null)
            {
                return true;
            }
            var blacklistedToken = await _context.BlacklistedTokens.ToListAsync();
            if (blacklistedToken.Count == 0)
            {
                return false;
            }
            if(blacklistedToken.Any(t => t.Token == token))
            {
                return true;
            }
            return false;

        }
    }
}
