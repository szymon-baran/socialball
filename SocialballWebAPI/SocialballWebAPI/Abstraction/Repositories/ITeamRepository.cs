using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface ITeamRepository
    {
        List<Team> GetTeams();
        List<Team> GetTeamsByLeague(Guid leagueId);
        Team GetTeamDetails(Guid id);
        void AddTeam(Team team);
        void UpdateTeam(Team team);
        void RemoveTeam(Team team);
        List<Team> AdminGetTeamsByLeague(Guid? leagueId);
    }
}
