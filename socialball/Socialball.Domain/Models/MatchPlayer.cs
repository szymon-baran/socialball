using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class MatchPlayer
    {
        public MatchPlayer()
        {
            MatchEvents = new HashSet<MatchEvent>();
        }

        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MatchId { get; set; }
        public Guid TeamId { get; set; }
        public PositionType Position { get; set; }
        public int? Number { get; set; }

        public virtual Player Player { get; set; }
        public virtual Match Match { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<MatchEvent> MatchEvents { get; set; }
    }
}
