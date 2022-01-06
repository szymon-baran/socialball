using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMatchPlayerRepository
    {
        List<MatchPlayer> GetMatchPlayersInMatch(Guid matchId);
        List<MatchPlayer> GetMatchPlayersInMatchByTeam(Guid matchId, Guid teamId);
        int? GetPlayerMatchesCount(Guid playerId);
        void AddMatchPlayer(MatchPlayer matchPlayer);
    }
}
