using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class FromUserJobAdvertisement : JobAdvertisement
    {
        public Guid UserId { get; set; }
        public List<League> WantedLeagues { get; set; }
        public User User { get; set; }
    }
}
