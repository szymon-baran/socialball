using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class MatchService : IMatchService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public MatchService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public object GetAllMatches()
        {
            return _context.Matches.Include(x => x.MatchEvents).ThenInclude(x => x.Player).ToList();
        }

        public object GetTeamMatches(Guid teamId)
        {
            return _context.Matches.Where(x => x.HomeTeamId == teamId || x.AwayTeamId == teamId).Include(x => x.MatchEvents).ThenInclude(x => x.Player).ToList();
        }

        public Match GetMatchDetails(Guid id)
        {
            Match match = _context.Matches
                    .Include(x => x.MatchEvents)
                        .ThenInclude(x => (x as MatchEventGoal).AssistPlayer)
                    .Include(x => x.MatchEvents)
                        .ThenInclude(x => x.Player)
                    .Include(x => x.AwayTeam)
                        .ThenInclude(x => x.League)
                    .Include(x => x.HomeTeam)
                        .ThenInclude(x => x.League)
                .FirstOrDefault(x => x.Id == id);

            if (match == null)
            {
                throw new KeyNotFoundException();
            }

            return match;
        }
    }
}
