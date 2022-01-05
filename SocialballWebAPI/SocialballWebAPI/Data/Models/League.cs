using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class League
    {
        public League()
        {
            Teams = new HashSet<Team>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int? Ranking { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
