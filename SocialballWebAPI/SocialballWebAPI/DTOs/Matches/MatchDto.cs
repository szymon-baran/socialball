using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class MatchDto
    {
        public Guid? Id { get; set; }
        public Guid? HomeTeamId { get; set; }
        public Guid? AwayTeamId { get; set; }
        public Guid? AddedByTeamId { get; set; }
        public string Stadium { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsConfirmed { get; set; }
        public MatchType MatchType { get; set; }
        public List<MatchEventDto> MatchEvents { get; set; }
    }
}
