using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IPlayerTransferOfferRepository
    {
        List<PlayerTransferOffer> GetToTeamTransferOffers(Guid toTeamId);
        List<PlayerTransferOffer> GetFromTeamTransferOffers(Guid fromTeamId);
        List<PlayerTransferOffer> GetPlayerTransferOffers(Guid playerId);
        PlayerTransferOffer GetPlayerTransferOfferDetails(Guid id);
        void AddPlayerTransferOffer(PlayerTransferOffer playerTransferOffer);
        void UpdatePlayerTransferOffer(PlayerTransferOffer playerTransferOffer);
        void RemovePlayerTransferOffer(PlayerTransferOffer playerTransferOffer);
    }
}
