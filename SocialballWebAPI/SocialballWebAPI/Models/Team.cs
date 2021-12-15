using System;
using System.Collections.Generic;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class Team
    {
        public Team()
        {
            MatchAwayTeams = new HashSet<Match>();
            MatchHomeTeams = new HashSet<Match>();
            UserDatas = new HashSet<UserData>();
            FromTeamJobAdvertisements = new HashSet<FromTeamJobAdvertisement>();
            JobAdvertisementAnswers = new HashSet<JobAdvertisementAnswer>();
            FromTeamPlayerTransferOffers = new HashSet<PlayerTransferOffer>();
            ToTeamPlayerTransferOffers = new HashSet<PlayerTransferOffer>();
            TeamMatchEvents = new HashSet<MatchEvent>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LeagueId { get; set; }
        public int? Points { get; set; }

        public virtual League League { get; set; }
        public virtual ICollection<Match> MatchAwayTeams { get; set; }
        public virtual ICollection<Match> MatchHomeTeams { get; set; }
        public virtual ICollection<UserData> UserDatas { get; set; }
        public virtual ICollection<FromTeamJobAdvertisement> FromTeamJobAdvertisements { get; set; }
        public virtual ICollection<JobAdvertisementAnswer> JobAdvertisementAnswers { get; set; }
        public virtual ICollection<PlayerTransferOffer> FromTeamPlayerTransferOffers { get; set; }
        public virtual ICollection<PlayerTransferOffer> ToTeamPlayerTransferOffers { get; set; }
        public virtual ICollection<MatchEvent> TeamMatchEvents { get; set; }
    }
}
