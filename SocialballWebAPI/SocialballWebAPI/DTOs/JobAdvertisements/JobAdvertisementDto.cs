using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class JobAdvertisementDto
    {
        public Guid Id { get; set; }
        public JobAdvertisementType JobAdvertisementType { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }

        public Guid? TeamId { get; set; }
        public string TeamName { get; set; }
        public int? Earnings { get; set; }
        public int TrainingSessionsPerWeek { get; set; }
        public PositionType Position { get; set; }

        public Guid? UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
