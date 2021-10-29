using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class PrivateMessage : Message
    {
        public Guid ToUserId { get; set; }
        public virtual User ToUser { get; set; }
    }
}
