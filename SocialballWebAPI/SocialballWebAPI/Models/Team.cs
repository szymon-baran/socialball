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
            UserDatas = new HashSet<UserData>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LeagueId { get; set; }

        public virtual League League { get; set; }
        public virtual ICollection<Match> MatchAwayTeams { get; set; }
        public virtual ICollection<Match> MatchHomeTeams { get; set; }
        public virtual ICollection<UserData> UserDatas { get; set; }
    }
}
