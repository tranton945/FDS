using FDS.Data;

namespace FDS.Services
{
    public class UserRepository : IUser
    {
        private readonly FDSDbContext _context;

        public UserRepository(FDSDbContext context) 
        {
            _context = context;
        }
        public User Add(User user)
        {
            var _user = new User
            {
                UserName = user.UserName,
                Password= user.Password,
                Name= user.Name,
                age= user.age,
                gender= user.gender,
            };
            _context.Add(_user);
            _context.SaveChanges();
            return _user;
        }

        public void Delete(Guid id)
        {
            var User = _context.Users.SingleOrDefault(a => a.UserID == id);
            if(User != null)
            {
                _context.Remove(User);
                _context.SaveChanges();
            }
        }

        public List<User> GetAll()
        {
            var listUser = _context.Users.Select(a => a);
            return listUser.ToList();
        }

        public User GetById(Guid id)
        {
            var User = _context.Users.SingleOrDefault(a => a.UserID == id);
            if(User != null)
            {
                return User;
            }
            return null;
        }

        public void Update(User User)
        {
            var _User = _context.Users.SingleOrDefault(a => a.UserID == User.UserID);
            if (_User != null)
            {
                _User.UserName = User.UserName;
                _User.Password = User.Password;
                _User.age = User.age;
                _User.gender = User.gender;

                _context.Update(_User);
                _context.SaveChanges();
            }

        }
    }
}
