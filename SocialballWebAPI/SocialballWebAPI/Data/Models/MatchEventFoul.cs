using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class MatchEventFoul : MatchEvent
    {
        public PenaltyType? PenaltyType { get; set; }
    }
}
