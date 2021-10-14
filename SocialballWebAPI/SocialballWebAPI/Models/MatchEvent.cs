using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class MatchEvent
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid MatchId { get; set; }
        public int Minute { get; set; }
        public MatchEventType MatchEventType { get; set; }

        public virtual Player Player { get; set; }
        public virtual Match Match { get; set; }
    }
}
