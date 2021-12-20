using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs.Admins
{
    public class AdminEditTeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LeagueId { get; set; }
        public bool IsActive { get; set; }
    }
}
