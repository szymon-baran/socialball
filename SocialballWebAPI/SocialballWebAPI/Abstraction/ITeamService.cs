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
    public interface ITeamService
    {
        object GetTeams();
        void AddTeam(AddTeamDto model);
        object GetTeamsByLeague(Guid leagueId);
        List<SelectList> GetTeamsToSelectList();
        List<PositionsInTeam> GetTeamsToChart(Guid teamId);
        TeamDto GetTeamDetails(Guid id);
        List<League> GetLeaguesToLookup();
    }
}
