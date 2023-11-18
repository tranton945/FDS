using FDS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FDS.Services
{
    public class DocTypeRepository : IDocTypeRepository
    {
        private readonly FDSDbContext _context;
        private readonly GetUser _getUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocTypeRepository(FDSDbContext context, GetUser getUser, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _getUser = getUser;
            _userManager = userManager;
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

        public async Task<DocType> Add(DocType type)
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var user = await _getUser.user();
            var AllType  = await _context.DocTypes.ToListAsync();
            if (AllType.Any(doc => doc.DocumentType == type.DocumentType))
            {
                // Tên của type mới trùng với một type đã có
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
            if (await checkAccountPermission() == false)
            {
                return false;
            }
            var result = await _context.DocTypes
                                    .Include(q => q.GroupTypes)
                                    .Include(d => d.Documents)
                                    .SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            };
            _context.RemoveRange(result.GroupTypes);
            _context.RemoveRange(result.Documents);
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DocType>> GetAll()
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var result = await _context.DocTypes
                                    .Include(q => q.GroupTypes)
                                    //.Include(d => d.Documents)
                                    .ToListAsync();
            return result;
        }

        public async Task<DocType> GetById(int id)
        {
            if (await checkAccountPermission() == false)
            {
                return null;
            }
            var result = await _context.DocTypes
                                    .Include(q => q.GroupTypes)
                                    .Include(d => d.Documents)
                                    .SingleOrDefaultAsync(a => a.Id == id);
            if(result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> Update(DocType type, int id)
        {
            if (await checkAccountPermission() == false)
            {
                return false;
            }
            var result = await _context.DocTypes.Include(q => q.GroupTypes).SingleOrDefaultAsync(a => a.Id == id);
            if(result == null) 
            {
                return false; 
            };
            result.DocumentType = type.DocumentType ?? result.DocumentType;
            result.Note = type.Note ?? result.Note;
            result.Createtor = result.Createtor;
            result.CreateDate = result.CreateDate;

            //cập nhật  GroupType
            foreach (var updatedGroupType in type.GroupTypes)
            {
                var _result = result.GroupTypes.FirstOrDefault(gt => gt.Id == updatedGroupType.Id);

                if (_result != null)
                {
                    _result.GroupId = _result.GroupId;
                    _result.Permission = updatedGroupType.Permission ?? _result.Permission;
                    _result.DocTypeId = _result.DocTypeId;
                }
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
