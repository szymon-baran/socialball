using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class TeamJobAdvertisementDto
    {
        public Guid? Id { get; set; }
        public Guid TeamId { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public int LowestEarningsPerMonth { get; set; }
        public int HighestEarningsPerMonth { get; set; }
        public int TrainingSessionsPerWeek { get; set; }
        public PositionType Position { get; set; }
        public bool IsActive { get; set; }
    }
}
