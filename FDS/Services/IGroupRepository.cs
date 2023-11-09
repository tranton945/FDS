using FDS.Data;
using FDS.Models;

namespace FDS.Services
{
    public interface IGroupRepository
    {
        public Task<List<Group>> GetAll();
        public Task<Group> GetById(int id);
        public Task<Group> Add(Group g);
        public Task<Group> AddMember(List<string> username, int groupId);
        public Task<bool> Update(Group g, int id);
        public Task<bool> Delete(int id);
    }
}
