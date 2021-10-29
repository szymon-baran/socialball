using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class SendTeamMessageDto
    {
        public Guid Id { get; set; }
        public Guid? ToTeamId { get; set; }
        public Guid? FromUserId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
