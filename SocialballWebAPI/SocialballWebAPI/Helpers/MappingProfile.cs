using AutoMapper;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.MatchPlayers;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, GetPlayerDto>().ReverseMap();
            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<Message, SendMessageDto>().ReverseMap();
            CreateMap<UserData, GetUserDataDto>().ReverseMap();
            CreateMap<Match, MatchDetailsDto>().ReverseMap();
            CreateMap<PlayerTransferOffer, PlayerTransferOfferDto>().ReverseMap();
            CreateMap<PlayerTransferOffer, AddPlayerTransferOfferDto>().ReverseMap();
            CreateMap<MatchPlayer, MatchPlayerDto>().ReverseMap();
        }
    }
}
