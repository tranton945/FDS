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
        public async Task<Document> Add(Document doc, IFormFile file, IFormFile signature)
        {
            var user = await _getuser.user();

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

        //public async Task<bool> Update(Document doc, int id, IFormFile file, IFormFile signature)
        //{
        //    var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
        //    if (result == null)
        //    {
        //        return false;
        //    }
        //    if(result.Signature != null || result.Signature.Length > 0)
        //    {
        //        return false;
        //    }

        //    // create new OldDocVer
        //    var oldDoc = new OldDocVer
        //    {
        //        Name = result.Name,
        //        Note = result.Note,
        //        CreateDate = result.CreateDate,
        //        UpdateDate = DateTime.Now,
        //        Version = result.Version,
        //        Creator = result.Creator,
        //        Signature = result.Signature,
        //        DocFile = result.DocFile,
        //        DocId = result.Id
        //    };
        //    await _context.OldDocVers.AddAsync(oldDoc);

        //    if (result.DocFile != null || result.DocFile.Length > 0)
        //    {
        //        result.DocFile = await ConvertFormFileToByteArray(file);
        //    }
        //    result.Name = doc.Name;
        //    result.Note = doc.Note;

        //    result.Signature = await ConvertFormFileToByteArray(signature);

        //    result.UpdateDate = DateTime.Now;
        //    if(doc.Version == null || doc.Version == 0 || doc.Version == 1)
        //    {
        //        result.Version = result.Version + 0.1f;
        //    }
        //    else
        //    {
        //        result.Version = doc.Version;
        //    }
        //    result.Creator = result.Creator;
        //    if (doc.FlightId == null || doc.FlightId == 0)
        //    {
        //        result.FlightId = result.FlightId;
        //    }
        //    else
        //    {
        //        result.FlightId = doc.FlightId;
        //    }

        //    _context.Update(result);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}
        public async Task<bool> Update(Document doc, int id, IFormFile file, IFormFile signature)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            if (result.Signature != null && result.Signature.Length > 0)
            {
                return false;
            }


            // create new OldDocVer
            var oldDoc = new OldDocVer
            {
                Name = result.Name,
                Note = result.Note,
                CreateDate = result.CreateDate,
                UpdateDate = DateTime.Now,
                Version = result.Version,
                Creator = result.Creator,
                Signature = result.Signature,
                DocFile = result.DocFile,
                DocId = result.Id
            };
            await _context.OldDocVers.AddAsync(oldDoc);

            if (file != null && file.Length > 0)
            {
                result.DocFile = await ConvertFormFileToByteArray(file);
            }
            result.Name = doc.Name ?? result.Name;
            result.Note = doc.Note ?? result.Note;

            if (signature != null && signature.Length > 0)
            {
                result.Signature = await ConvertFormFileToByteArray(signature);
            }

            result.UpdateDate = DateTime.Now;
            if (doc.Version == null || doc.Version == 0 || doc.Version == 1)
            {
                result.Version = result.Version + 0.1f;
            }
            else
            {
                result.Version = doc.Version;
            }
            result.Creator = result.Creator;
            result.FlightId = doc.FlightId ?? result.FlightId;


            _context.Update(result);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
