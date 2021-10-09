using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Player
    {
        public Player()
        {
            GoalScorers = new HashSet<Goal>();
            GoalAssistPlayers = new HashSet<Goal>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Position { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<Goal> GoalScorers { get; set; }
        public virtual ICollection<Goal> GoalAssistPlayers { get; set; }
    }
}
