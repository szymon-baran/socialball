using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class Message
    {
        public Message()
        {
            MessageRecipients = new HashSet<UserMessage>();
        }

        public Guid Id { get; set; }
        public Guid? FromUserId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public MessageType MessageType { get; set; } 

        public virtual User FromUser { get; set; }
        public virtual ICollection<UserMessage> MessageRecipients { get; set; }
    }
}
