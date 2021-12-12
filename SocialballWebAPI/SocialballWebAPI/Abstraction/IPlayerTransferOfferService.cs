using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IPlayerTransferOfferService
    {
        List<PlayerTransferOfferDto> GetTeamTransferOffers(Guid teamId);
        List<PlayerTransferOfferDto> GetFromTeamTransferOffers(Guid teamId);
        List<PlayerTransferOfferDto> GetPlayerTransferOffers(Guid userId);
        void AddPlayerTransferOffer(AddPlayerTransferOfferDto model);
        PlayerTransferOfferDto GetPlayerTransferOfferDetails(Guid id);
        void RejectOffer(Guid id);
        void AcceptOfferAsTeam(Guid id);
        void AcceptOfferAsPlayer(Guid id);
    }
}
