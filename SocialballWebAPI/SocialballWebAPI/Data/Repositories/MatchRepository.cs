using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly SocialballDBContext _context;

        public MatchRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<Match> GetConfirmedMatches()
        {
            return _context.Matches.Where(x => x.IsConfirmed).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).ToList();
        }

        public List<Match> GetConfirmedTeamMatches(Guid teamId)
        {
            return _context.Matches.Where(x => x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).ToList();
        }

        public List<Match> GetUnconfirmedTeamMatches(Guid teamId)
        {
            return _context.Matches.Where(x => !x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).Include(x => x.HomeTeam).Include(x => x.AwayTeam).ToList();
        }

        public Match GetMatchDetails(Guid id)
        {
            return _context.Matches.Include(x => x.AwayTeam).ThenInclude(x => x.League).Include(x => x.HomeTeam).ThenInclude(x => x.League).Single(x => x.Id == id);
        }

        public void AddMatch(Match match)
        {
            _context.Matches.Add(match);
            _context.SaveChanges();
        }

        public void RemoveMatch(Match match)
        {
            _context.Matches.Remove(match);
            _context.SaveChanges();
        }
    }
}
