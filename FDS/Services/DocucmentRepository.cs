using FDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace FDS.Services
{
    public class DocucmentRepository : IDocument
    {
        private readonly FDSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocucmentRepository(FDSDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Document> Add(Document doc)
        {
            // lấy jwt mà user cung cấp
            var authResult = _httpContextAccessor.HttpContext.AuthenticateAsync().Result;
            var token = authResult.Properties.GetTokenValue("access_token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            // lấy email từ chuỗi jwt sau khi giải mã jwt
            var _email = jwtToken.Claims.FirstOrDefault().Value;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _email);

            var _doc = new Document
            {
                Name = doc.Name,
                Note = doc.Note,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Version = 1,
                Creator = user.Name,
                Signature = doc.Signature,
                FlightId = doc.FlightId,
            };
            await _context.AddAsync(_doc);
            await _context.SaveChangesAsync();
            return _doc;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Document>> GetAll()
        {
            var result = await _context.Documents.ToListAsync();
            return result;
        }

        public async Task<Document> GetById(int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> Update(Document doc, int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            else
            {
                var oldDoc = new OldDocVer
                {
                    Name = result.Name,
                    Note = result.Note,
                    CreateDate = result.CreateDate,
                    UpdateDate = DateTime.Now,
                    Version = result.Version,
                    Creator = result.Creator,
                    Signature = result.Signature,
                    DocId = result.Id
                };
                await _context.OldDocVers.AddAsync(oldDoc);
                //await _context.SaveChangesAsync();
            }

            result.Name = doc.Name;
            result.Note = doc.Note;
            //result.CreateDate = result.CreateDate;
            result.UpdateDate = DateTime.Now;
            if(doc.Version == null || doc.Version == 0)
            {
                result.Version = result.Version + 0.1f;
            }
            else
            {
                result.Version = doc.Version;
            }
            result.Creator = result.Creator;
            result.Signature = doc.Signature;
            if (doc.FlightId == null || doc.FlightId == 0)
            {
                result.FlightId = result.FlightId;
            }
            else
            {
                result.FlightId = doc.FlightId;
            }

            _context.Update(result);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
