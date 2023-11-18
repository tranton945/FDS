using FDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FDS.Helper;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace FDS.Services
{
    public class DocucmentRepository : IDocument
    {
        private readonly FDSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GetUser _getuser;

        public DocucmentRepository(FDSDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, GetUser getuser)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _getuser = getuser;
        }

        public async Task<bool> checkAdminAccount()
        {
            var user = await _getuser.user();
            var adminRole = await _userManager.GetRolesAsync(user);
            if (adminRole.Any(a => a.ToUpper() == "ADMIN"))
            {
                return true;
            }
            return false;
        }
        public async Task<string> checkPermission(int? docId)
        {
            var groupType = await _getuser.ListGroupsType();
            if (groupType.Count == 0)
            {
                return "NO PERMISSION";
            }
            var _getPermission = groupType.Select(g => g.DocTypeId == docId).ToList();
            var getPermission = groupType.SingleOrDefault(g => g.DocTypeId == docId);
            if (getPermission.Permission == null || getPermission.Permission.ToUpper() == "NO PERMISSION" || getPermission == null)
            {
                return "NO PERMISSION";
            }
            return getPermission.Permission.ToUpper();
        }
        public async Task<Document> Add(Document doc, IFormFile file, IFormFile signature)
        {
            if(await checkAdminAccount() == true)
            {
                var result = await AddSecondary(doc, file, signature);
                return result;
            }
            if(await checkPermission(doc.DocTypeId) == "NO PERMISSION") 
            {
                return null;
            }
            var _result = await AddSecondary(doc, file, signature);

            return _result;
        }

        public async Task<Document> AddSecondary(Document doc, IFormFile file, IFormFile signature)
        {
            var user = await _getuser.user();
            var check = await _context.Documents.ToListAsync();
            if (check.Any(checkname => checkname.Name == doc.Name))
            {
                // Tên của Doc mới trùng với một Doc đã có
                return null;
            }
            var _doc = new Document
            {
                Name = doc.Name,
                Note = doc.Note,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Version = 1,
                Creator = user.UserName,
                FlightId = doc.FlightId,
                DocTypeId = doc.DocTypeId,
            };
            if (file != null && file.Length > 0)
            {
                _doc.DocFile = await ConvertFormFileToByteArray(file);
            }
            else
            {
                _doc.DocFile = null;
            }
            if (signature != null && signature.Length > 0)
            {
                _doc.Signature = await ConvertFormFileToByteArray(signature);
            }
            else
            {
                _doc.Signature = null;
            }

            await _context.AddAsync(_doc);
            await _context.SaveChangesAsync();
            return _doc;
        }

        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            if (await checkAdminAccount() == true)
            {
                _context.Documents.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            if (await checkPermission(result.DocTypeId) == "READ ONLY" || await checkPermission(result.DocTypeId) == "NO PERMISSION")
            {
                return false;
            }

            var accounts = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == result.Creator);
            var userRole = await _userManager.GetRolesAsync(accounts);
            bool checkRole = userRole.Any(role => role.ToUpper() == "ADMIN" || role.ToUpper() == "GO");
            if (!checkRole)
            {
                return false;
            }
            _context.Documents.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Document>> GetAll()
        {
            if (await checkAdminAccount() == false)
            {
                return null;
            }
            var result = await _context.Documents.ToListAsync();
            return result;
        }

        public async Task<Document> GetById(int id)
        {

            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return null;
            }
            if (await checkAdminAccount() == true)
            {
                return result;
            }
            if (await checkPermission(result.DocTypeId) == "NO PERMISSION")
            {
                return null;
            }

            return result;
        }

        public async Task<List<Document>> GetDocumentsByAuthor(string authorName)
        {
            var result = await _context.Documents
                    .Where(doc => doc.Creator.Contains(authorName))
                    .OrderByDescending(n => n.Id)
                    .ToListAsync();
            return result;
        }

        public async Task<List<Document>> GetDocumentsByDate()
        {
            var result = await _context.Documents
                .OrderByDescending(doc => doc.CreateDate)
                .ToListAsync();

            return result;
        }

        public async Task<List<Document>> Search(string searchString)
        {
            var result = await _context.Documents
                                        .Where(s => s.Name.Contains(searchString) ||
                                        s.Note.Contains(searchString))
                                        .OrderBy(n => n.Id)
                                        .ToListAsync();
            return result;
        }

        public async Task<bool> Update(Document doc, int id, IFormFile file, IFormFile signature)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            if (await checkAdminAccount() == true)
            {
                var Update = await UpdateSecondary(doc, id, file, signature, result);
                return Update;
            }

            if (await checkPermission(result.DocTypeId) == "READ ONLY" || await checkPermission(result.DocTypeId) == "NO PERMISSION")
            {
                return false;
            }
            var _Update = await UpdateSecondary(doc, id, file, signature, result);
            return _Update;
        }
        public async Task<bool> UpdateSecondary(Document doc, int id, IFormFile file, IFormFile signature, Document availableDoc)
        {
            if (availableDoc.Signature != null && availableDoc.Signature.Length > 0)
            {
                return false;
            }
            var check = await _context.Documents.ToListAsync();
            if (check.Any(checkname => checkname.Name == doc.Name))
            {
                // Tên của Doc mới trùng với một Doc đã có
                return false;
            }

            // create new OldDocVer
            var oldDoc = new OldDocVer
            {
                Name = availableDoc.Name,
                Note = availableDoc.Note,
                CreateDate = availableDoc.CreateDate,
                UpdateDate = DateTime.Now,
                Version = availableDoc.Version,
                Creator = availableDoc.Creator,
                Signature = availableDoc.Signature,
                DocFile = availableDoc.DocFile,
                DocId = availableDoc.Id
            };
            await _context.OldDocVers.AddAsync(oldDoc);

            if (file != null && file.Length > 0)
            {
                availableDoc.DocFile = await ConvertFormFileToByteArray(file);
            }
            availableDoc.Name = doc.Name ?? availableDoc.Name;
            availableDoc.Note = doc.Note ?? availableDoc.Note;

            if (signature != null && signature.Length > 0)
            {
                availableDoc.Signature = await ConvertFormFileToByteArray(signature);
            }

            availableDoc.UpdateDate = DateTime.Now;
            if (doc.Version == null || doc.Version <= availableDoc.Version)
            {
                availableDoc.Version = availableDoc.Version + 0.1f;
            }
            else
            {
                availableDoc.Version = doc.Version;
            }
            availableDoc.Creator = availableDoc.Creator;
            availableDoc.FlightId = doc.FlightId ?? availableDoc.FlightId;

            _context.Update(availableDoc);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Document>> GetDocumentsByFilghtId(int flightId)
        {
            var result = await _context.Documents
                    .Where(doc => doc.FlightId == flightId)
                    .ToListAsync();
            return result;
        }

        public async Task<Document> DowloadDocumentById(int id)
        {
            var document = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);

            if (document == null || document.DocFile == null || document.DocFile.Length <= 0)
            {
                return null;
            }
            return document;

        }

        public async Task<List<Data.Document>> DowloadDocumentByFlightId(int id)
        {
            var result = await _context.Documents
                .Where(doc => doc.FlightId == id)
                .ToListAsync();
            if(result.Count == 0)
            {
                return null;
            }
            return result;
        }
    }
}
