using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class MatchEventGoal : MatchEvent
    {
        public Guid? AssistPlayerId { get; set; }
        public virtual Player AssistPlayer { get; set; }
    }
}
