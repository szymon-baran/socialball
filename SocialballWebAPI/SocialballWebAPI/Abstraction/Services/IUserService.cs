using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.Teams;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        User GetById(Guid id);
        Guid AddUserAccountForNewPlayer(RegisterPlayerDto playerModel);
        Guid AddUserAccountForNewTeamManager(AddTeamDto model);
        List<SelectList> GetUsersToLookup();
    }
}
