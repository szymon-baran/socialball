using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialballDBContext _context;

        public UserRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<User> GetPlayersUsers()
        {
            return _context.Users.Include(x => x.UserData).Where(x => x.UserData.UserType == UserType.Player).ToList();
        }

        public User GetUserDetails(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserDetailsByUsername(string username)
        {
            return _context.Users.Include(x => x.UserData).SingleOrDefault(x => x.Username == username);
        }

        public bool CheckUsernameUniqueness(string username)
        {
            return !_context.Users.Any(x => x.Username == username);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        #region Admin

        public List<User> AdminGetUsers()
        {
            return _context.Users.Include(x => x.UserData).Where(x => x.UserData.UserType != UserType.System).OrderBy(x => x.UserData.UserType).ToList();
        }

        #endregion
    }
}
