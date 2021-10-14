using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class MatchDto
    {
        // TO DO - a moze nie? https://stackoverflow.com/questions/31101339/how-can-i-fetch-child-entities-as-dto-in-parent-using-reusable-queries-expressio
        public MatchDto()
        {
            MatchEvents = new HashSet<MatchEvent>();
        }

        public Guid Id { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public string Stadium { get; set; }
        public DateTime DateTime { get; set; }

        public virtual Team AwayTeam { get; set; }
        public virtual Team HomeTeam { get; set; }
        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
    }
}
