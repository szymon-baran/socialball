using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class GoalInPlayerDetailsDto
    {
        public Guid Id { get; set; }
        public Guid MatchId { get; set; }
        public int Minute { get; set; }
        public string MatchBetween { get; set; }
        public DateTime DateTime { get; set; }
    }
}
