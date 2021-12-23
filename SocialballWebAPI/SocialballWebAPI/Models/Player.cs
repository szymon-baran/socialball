using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public class Player : UserData
    {
        public Player()
        {
            MatchesPlayer = new HashSet<MatchPlayer>();
            MatchGoalsAssisted = new HashSet<MatchEventGoal>();
            PlayerTransferOffers = new HashSet<PlayerTransferOffer>();
        }
        public int? Position { get; set; }
        public DateTime? IsInjuredUntil { get; set; }

        public virtual ICollection<MatchPlayer> MatchesPlayer { get; set; }
        public virtual ICollection<MatchEventGoal> MatchGoalsAssisted { get; set; }
        public virtual ICollection<PlayerTransferOffer> PlayerTransferOffers { get; set; }
    }
}
