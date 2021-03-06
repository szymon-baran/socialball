using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
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
        GetUserDataDto GetUserDataDetails(Guid id);
        GetPlayerDto GetPlayerDetailsByUserId(Guid userId);
        GetUserDataDto GetUserDataByUserId(Guid userId);
        Guid? GetUserTeamId(Guid userId);
        List<SelectList> GetPlayersByTeamToLookup(Guid teamId);
        void AddPlayer(RegisterPlayerDto playerModel);
        void EditUserData(EditPlayerDto model);
        void AddEditPlayerInjury(PlayerInjuryDto model);
        void KickPlayerOutOfTeam(Guid id);
        bool CheckUsernameUniqueness(string username);
    }
}
