using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Match
    {
        public Match()
        {
            Goals = new HashSet<Goal>();
        }

        public Guid Id { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public string Stadium { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Team AwayTeam { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
    }
}
