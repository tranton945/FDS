using FDS.Data;

namespace FDS.Services
{
    public interface IUser
    {
        List<User> GetAll();
        User GetById(Guid id);
        User Add(User user);
        void Update(User User, Guid id);
        void Delete(Guid id);
    }
}
