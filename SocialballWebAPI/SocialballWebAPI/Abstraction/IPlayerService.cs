using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IPlayerService
    {
        object GetPlayers();
        object GetPlayersByTeamId(Guid teamId);
        GetPlayerDto GetPlayerDetails(Guid id);
        GetPlayerDto GetPlayerDetailsByUserId(Guid userId);
        UserData GetUserDataByUserId(Guid userId);
        Guid? GetUserTeamId(Guid userId);
        void AddPlayer(RegisterPlayerDto playerModel);
        bool CheckUsernameUniqueness(string username);
    }
}
