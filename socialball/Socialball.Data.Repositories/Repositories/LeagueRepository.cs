using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly SocialballDBContext _context;

        public LeagueRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<League> GetLeagues()
        {
            return _context.Leagues.OrderBy(x => x.Ranking).ToList();
        }

    }
}
