using FDS.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FDS.Services
{
    public class DocTypeRepository : IDocTypeRepository
    {
        private readonly FDSDbContext _context;
        private readonly GetUser _getUser;

        public DocTypeRepository(FDSDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }
        public async Task<DocType> Add(DocType type)
        {
            var user = await _getUser.user();
            var AllType  = await _context.DocTypes.ToListAsync();
            if (AllType.Any(doc => doc.DocumentType == type.DocumentType))
            {
                // Tên của Doc mới trùng với một Doc đã có
                return null;
            }

            var _type = new DocType
            {
                DocumentType = type.DocumentType,
                Note = type.Note,
                Createtor = user.UserName,
                CreateDate = DateTime.Now,
                GroupTypes = type.GroupTypes.Select(gt => new GroupType
                {
                    GroupId = gt.GroupId,
                    Permission = gt.Permission,
                }).ToList(),
            };
            _context.DocTypes.Add(_type);
            await _context.SaveChangesAsync();

            var typeId = _type.Id;
            // Thiết lập TypeId cho mỗi GroupType
            foreach (var groupType in type.GroupTypes)
            {
                groupType.DocTypeId = typeId;
            }

            await _context.SaveChangesAsync();


            return _type;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.DocTypes.Include(q => q.GroupTypes).SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            };
            _context.RemoveRange(result.GroupTypes);
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DocType>> GetAll()
        {
            var result = await _context.DocTypes.ToListAsync();
            return result;
        }

        public async Task<DocType> GetById(int id)
        {
            var result = await _context.DocTypes
                                    .Include(q => q.GroupTypes)
                                    .SingleOrDefaultAsync(a => a.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> Update(DocType type, int id)
        {
            var result = await _context.DocTypes.SingleOrDefaultAsync(a => a.Id == id);
            if(result == null) 
            {
                return false; 
            };
            result.DocumentType = type.DocumentType;
            result.Note= type.Note;
            result.Createtor = result.Createtor;
            result.CreateDate = result.CreateDate;
            _context.DocTypes.Update(result);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
