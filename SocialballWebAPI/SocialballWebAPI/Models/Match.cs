using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Match
    {
        public Match()
        {
            MatchPlayers = new HashSet<MatchPlayer>();
        }

        public Guid Id { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public Guid? AddedByTeamId { get; set; }
        public string Stadium { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsConfirmed { get; set; }
        public MatchType MatchType { get; set; }

        public virtual Team AwayTeam { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
    }
}
