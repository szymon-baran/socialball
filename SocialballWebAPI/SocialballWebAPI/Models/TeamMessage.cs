using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class TeamMessage
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
