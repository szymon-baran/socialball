using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class PlayerInjuryDto
    {
        public Guid Id { get; set; }
        public DateTime IsInjuredUntil { get; set; }
    }
}
