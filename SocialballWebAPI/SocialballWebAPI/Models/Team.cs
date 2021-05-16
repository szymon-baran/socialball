﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Team
    {
        public Team()
        {
            MatchAwayTeams = new HashSet<Match>();
            MatchHomeTeams = new HashSet<Match>();
            Players = new HashSet<Player>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Match> MatchAwayTeams { get; set; }
        public virtual ICollection<Match> MatchHomeTeams { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
