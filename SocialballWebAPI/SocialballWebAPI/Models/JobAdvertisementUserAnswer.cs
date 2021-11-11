using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class JobAdvertisementUserAnswer : JobAdvertisementAnswer
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
