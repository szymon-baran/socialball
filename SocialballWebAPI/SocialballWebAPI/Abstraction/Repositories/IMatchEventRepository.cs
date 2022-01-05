using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMatchEventRepository
    {
        List<MatchEventGoal> GetGoalsByPlayer(Guid playerId);
        List<MatchEventGoal> GetAssistsByPlayer(Guid playerId);
        List<MatchEvent> GetMatchEventsInMatch(Guid matchId);
        List<MatchEventGoal> GetGoalsInMatchByTeam(Guid matchId, Guid teamId);
        ILookup<int, MatchEventGoal> GetGoalsPerMonthByPlayer(Guid playerId);
        void AddMatchEventGoal(MatchEventGoal matchEventGoal);
        void AddMatchEventFoul(MatchEventFoul matchEventFoul);
    }
}
