using FDS.Data;
using Microsoft.EntityFrameworkCore;

namespace FDS.Services
{
    public class AccountPermissionRepository : IAccountPermission
    {
        private readonly FDSDbContext _context;

        public AccountPermissionRepository(FDSDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> AddAccount(List<string> listUserName)
        {
            var listUser = await _context.AccountPermissions.ToListAsync();
            if(listUser != null || listUser.Count != 0)
            {
                foreach (var item in listUserName)
                {
                    bool checkUser = listUser.Any(u => u.UsreName == item);
                    if (!checkUser)
                    {
                        await _context.AccountPermissions.AddAsync(new Data.AccountPermission
                        {
                            UsreName= item,
                            AllowPermission = true
                        });
                    }
                }
            }
            else
            {
                foreach (var item in listUserName)
                {
                    await _context.AccountPermissions.AddAsync(new Data.AccountPermission
                    {
                        UsreName = item,
                        AllowPermission = true
                    });
                }
            }
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(string UserName)
        {
            var User = await _context.AccountPermissions.SingleOrDefaultAsync(a => a.UsreName == UserName);
            if(User == null)
            {
                return false;
            }
            _context.Remove(User);
            _context.SaveChanges(); 
            return true;
        }

        public async Task<List<Data.AccountPermission>> GetAll()
        {
            var result = await _context.AccountPermissions.ToListAsync();
            return result;
        }


        public async Task<Data.AccountPermission> GetByUserName(string UserName)
        {
            var User = await _context.AccountPermissions.SingleOrDefaultAsync(a => a.UsreName == UserName);
            if (User == null)
            {
                return null;
            }
            return User;
        }
    }
}
