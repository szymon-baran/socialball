using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SocialballDBContext _context;

        public TeamRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<Team> GetTeams()
        {
            return _context.Teams.Include(x => x.League).Where(x => x.IsActive).ToList();
        }

        public List<Team> GetTeamsByLeague(Guid leagueId)
        {
            return _context.Teams.Include(x => x.League).Where(x => x.IsActive && x.LeagueId == leagueId).ToList();
        }

        public Team GetTeamDetails(Guid id)
        {
            return _context.Teams.Include(x => x.League).FirstOrDefault(x => x.Id == id);
        }

        public void AddTeam(Team team)
        {
            _context.Teams.Add(team);
            _context.SaveChanges();
        }

        public void UpdateTeam(Team team)
        {
            _context.Teams.Update(team);
            _context.SaveChanges();
        }

        public void RemoveTeam(Team team)
        {
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        #region Admin

        public List<Team> AdminGetTeamsByLeague(Guid? leagueId)
        {
            return _context.Teams.Where(x => !leagueId.HasValue || x.LeagueId == leagueId).ToList();
        }

        #endregion

    }
}
