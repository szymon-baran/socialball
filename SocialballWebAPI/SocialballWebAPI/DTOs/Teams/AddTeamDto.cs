using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs.Teams
{
    public class AddTeamDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid LeagueId { get; set; }
        public byte[] Image { get; set; }
        public string LoginUsername { get; set; }
        public string Email { get; set; }
        public string LoginPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
