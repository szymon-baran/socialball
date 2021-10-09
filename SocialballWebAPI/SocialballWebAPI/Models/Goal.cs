using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class Goal
    {
        public Guid Id { get; set; }
        public Guid ScorerId { get; set; }
        public Guid? AssistPlayerId { get; set; }
        public Guid MatchId { get; set; }
        public int Minute { get; set; }

        public virtual Player Scorer { get; set; }
        public virtual Player AssistPlayer { get; set; }
        public virtual Match Match { get; set; }
    }
}
