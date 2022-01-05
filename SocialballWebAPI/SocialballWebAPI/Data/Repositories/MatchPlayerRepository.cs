using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class MatchPlayerRepository : IMatchPlayerRepository
    {
        private readonly SocialballDBContext _context;

        public MatchPlayerRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<MatchPlayer> GetMatchPlayersInMatch(Guid matchId)
        {
            return _context.MatchPlayers.Where(x => x.MatchId == matchId).ToList();
        }

        public List<MatchPlayer> GetMatchPlayersInMatchByTeam(Guid matchId, Guid teamId)
        {
            return _context.MatchPlayers.Include(x => x.Player).Where(x => x.MatchId == matchId && x.TeamId == teamId).ToList();
        }

        public void AddMatchPlayer(MatchPlayer matchPlayer)
        {
            _context.MatchPlayers.Add(matchPlayer);
            _context.SaveChanges();
        }
    }
}
