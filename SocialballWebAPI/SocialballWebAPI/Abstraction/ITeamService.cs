using SocialballWebAPI.DTOs;
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
        List<SelectList> GetTeamsToSelectList();
        List<PositionsInTeam> GetTeamsToChart(Guid teamId);
        TeamDto GetTeamDetails(Guid id);
        List<League> GetLeaguesToLookup();
    }
}
