using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class AddPlayerTransferOfferDto
    {
        public Guid PlayerId { get; set; }
        public Guid FromTeamId { get; set; }
        public Guid ToTeamId { get; set; }
        public string Content { get; set; }
        public int TransferFee { get; set; }
    }
}
