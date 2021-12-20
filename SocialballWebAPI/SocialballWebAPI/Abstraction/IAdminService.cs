using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.Admins;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IAdminService
    {
        object GetUsers();
        object GetTeams(Guid? leagueId);
        void EditTeam(AdminEditTeamDto model);
        void TeamDeleteAdmin(Guid id);
        void TeamImageDeleteAdmin(Guid id);
    }
}
