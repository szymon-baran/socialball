using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public class Player : UserData
    {
        public Player()
        {
            MatchEvents = new HashSet<MatchEvent>();
            MatchGoalsAssisted = new HashSet<MatchEventGoal>();
        }
        public int? Position { get; set; }

        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
        public virtual ICollection<MatchEventGoal> MatchGoalsAssisted { get; set; }
    }
}
