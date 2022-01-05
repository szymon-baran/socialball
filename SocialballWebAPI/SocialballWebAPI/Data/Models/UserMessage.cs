using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class UserMessage
    {
        public UserMessage()
        {
            IsActive = true;
            IsRead = false;
        }

        public Guid Id { get; set; }
        public Guid ToUserId { get; set; }
        public Guid MessageId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRead { get; set; }

        public virtual User ToUser { get; set; }
        public virtual Message Message { get; set; }
    }
}
