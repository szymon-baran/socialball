using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMatchRepository
    {
        List<Match> GetConfirmedMatches();
        List<Match> GetConfirmedTeamMatches(Guid teamId);
        List<Match> GetUnconfirmedTeamMatches(Guid teamId);
        Match GetMatchDetails(Guid id);
        void AddMatch(Match match);
        void RemoveMatch(Match match);
    }
}
