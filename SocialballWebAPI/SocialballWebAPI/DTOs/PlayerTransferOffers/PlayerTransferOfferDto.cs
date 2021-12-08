using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class PlayerTransferOfferDto
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid FromTeamId { get; set; }
        public Guid ToTeamId { get; set; }
        public string Content { get; set; }
        public int TransferFee { get; set; }
        public bool IsAcceptedByPlayer { get; set; }
        public bool IsAcceptedByOtherTeam { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual string FromTeamName { get; set; }
        public virtual string ToTeamName { get; set; }
    }
}
