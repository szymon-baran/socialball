using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
