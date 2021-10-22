using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Player
    {
        public Player()
        {
            MatchEvents = new HashSet<MatchEvent>();
            MatchGoalsAssisted = new HashSet<MatchEventGoal>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Position { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public Guid? UserId { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
        public virtual ICollection<MatchEventGoal> MatchGoalsAssisted { get; set; }
    }
}
