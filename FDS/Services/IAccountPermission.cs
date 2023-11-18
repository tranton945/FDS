using FDS.Data;

namespace FDS.Services
{
    public interface IAccountPermission
    {
        public Task<bool> AddAccount(List<string> listUserName);
        public Task<List<AccountPermission>> GetAll();
        public Task<AccountPermission> GetByUserName(string UserName);
        public Task<bool> Delete(string UserName);

    }
}
