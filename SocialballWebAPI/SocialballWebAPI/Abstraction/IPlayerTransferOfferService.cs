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
        List<PlayerTransferOffer> GetPlayerTransferOffers(Guid userId);
        void AddPlayerTransferOffer(AddPlayerTransferOfferDto model);
    }
}
