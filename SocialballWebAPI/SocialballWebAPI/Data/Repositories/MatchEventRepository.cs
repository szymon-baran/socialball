using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class MatchEventRepository : IMatchEventRepository
    {
        private readonly SocialballDBContext _context;

        public MatchEventRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<MatchEventGoal> GetGoalsByPlayer(Guid playerId)
        {
            return _context.MatchEventGoals
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.HomeTeam)
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.AwayTeam)
                .Include(x => x.AssistPlayer)
                .Where(x => x.MatchPlayer.PlayerId == playerId && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).ToList();
        }

        public List<MatchEventGoal> GetAssistsByPlayer(Guid playerId)
        {
            return _context.MatchEventGoals
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.HomeTeam)
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.AwayTeam)
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Player)
                .Include(x => x.AssistPlayer)
                .Where(x => x.AssistPlayerId == playerId && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).ToList();
        }

        public List<MatchEvent> GetMatchEventsInMatch(Guid matchId)
        {
            return _context.MatchEvents.Include(x => x.MatchPlayer).ThenInclude(x => x.Player).Include(x => ((MatchEventGoal)x).AssistPlayer).Where(x => x.MatchPlayer.MatchId == matchId).ToList();
        }

        public List<MatchEventGoal> GetGoalsInMatchByTeam(Guid matchId, Guid teamId)
        {
            return _context.MatchEventGoals.Include(x => x.MatchPlayer).Where(x => x.MatchPlayer.MatchId == matchId && x.MatchPlayer.TeamId == teamId).ToList();
        }

        public ILookup<int, MatchEventGoal> GetGoalsPerMonthByPlayer(Guid playerId)
        {
            return _context.MatchEventGoals.Include(x => x.MatchPlayer).ThenInclude(x => x.Match).Where(x => x.MatchPlayer.PlayerId == playerId && x.MatchPlayer.Match.DateTime.Year == DateTime.Today.Year && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).ToLookup(y => y.MatchPlayer.Match.DateTime.Month);
        }

        public void AddMatchEventGoal(MatchEventGoal matchEventGoal)
        {
            _context.MatchEventGoals.Add(matchEventGoal);
            _context.SaveChanges();
        }

        public void AddMatchEventFoul(MatchEventFoul matchEventFoul)
        {
            _context.MatchEventFouls.Add(matchEventFoul);
            _context.SaveChanges();
        }

    }
}
