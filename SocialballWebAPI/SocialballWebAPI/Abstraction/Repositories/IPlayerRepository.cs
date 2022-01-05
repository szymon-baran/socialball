using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IPlayerRepository
    {
        object GetPlayers();
        List<Player> GetPlayersInTeam(Guid teamId);
        List<UserData> GetUserDatasInTeam(Guid teamId);
        List<UserData> GetManagersInTeam(Guid teamId);
        Player GetPlayerDetails(Guid id);
        Player GetPlayerDetailsByUserId(Guid userId);
        UserData GetUserDataDetails(Guid id);
        UserData GetUserDataDetailsByUserId(Guid userId);
        UserData GetSystemUserDetails();
        void AddPlayer(Player player);
        void AddTeamManager(TeamManager teamManager);
        void UpdatePlayer(Player player);
        void UpdateUserData(UserData userData);
    }
}
