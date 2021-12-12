using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class FromTeamJobAdvertisement : JobAdvertisement
    {
        public Guid TeamId { get; set; }
        public int TrainingSessionsPerWeek { get; set; }
        public PositionType Position { get; set; }
        public Team Team { get; set; }
    }
}
