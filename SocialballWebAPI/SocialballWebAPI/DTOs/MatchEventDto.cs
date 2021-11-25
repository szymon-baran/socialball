using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class MatchEventDto
    {
        public Guid? Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid? MatchId { get; set; }
        public int Minute { get; set; }
        public MatchEventType MatchEventType { get; set; }
        public Guid? AssistPlayerId { get; set; }
        public PenaltyType? PenaltyType { get; set; }
    }
}
