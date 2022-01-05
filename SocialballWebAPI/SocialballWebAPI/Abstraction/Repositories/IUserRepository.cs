using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IUserRepository
    {
        List<User> GetPlayersUsers();
        User GetUserDetails(Guid id);
        User GetUserDetailsByUsername(string username);
        bool CheckUsernameUniqueness(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        List<User> AdminGetUsers();
    }
}
