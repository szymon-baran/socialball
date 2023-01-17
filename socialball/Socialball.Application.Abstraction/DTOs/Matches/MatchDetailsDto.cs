using SocialballWebAPI.DTOs.MatchEvents;
using SocialballWebAPI.DTOs.MatchPlayers;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class MatchDetailsDto
    {
        public Guid Id { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public Guid? AddedByTeamId { get; set; }
        public string Stadium { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsConfirmed { get; set; }
        public MatchType MatchType { get; set; }

        public virtual TeamDto AwayTeam { get; set; }
        public virtual TeamDto HomeTeam { get; set; }
        public GetMatchPlayerDto HomeMatchGoalkeeper { get; set; }
        public List<GetMatchPlayerDto> HomeMatchDefenders { get; set; }
        public List<GetMatchPlayerDto> HomeMatchMidfielders { get; set; }
        public List<GetMatchPlayerDto> HomeMatchStrikers { get; set; }
        public GetMatchPlayerDto AwayMatchGoalkeeper { get; set; }
        public List<GetMatchPlayerDto> AwayMatchDefenders { get; set; }
        public List<GetMatchPlayerDto> AwayMatchMidfielders { get; set; }
        public List<GetMatchPlayerDto> AwayMatchStrikers { get; set; }
        public List<GetMatchEventDto> MatchEvents { get; set; }

    }
}
