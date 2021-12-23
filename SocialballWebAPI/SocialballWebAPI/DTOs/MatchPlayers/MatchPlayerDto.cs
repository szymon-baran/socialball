using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs.MatchPlayers
{
    public class MatchPlayerDto
    {
        public Guid? Id { get; set; }
        public Guid PlayerId { get; set; }
        public PositionType Position { get; set; }
        public int? Number { get; set; }

        public List<MatchEventDto> MatchEvents { get; set; }

    }
}
