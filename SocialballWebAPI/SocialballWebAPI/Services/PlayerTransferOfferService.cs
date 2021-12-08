using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class PlayerTransferOfferService : IPlayerTransferOfferService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public PlayerTransferOfferService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<PlayerTransferOfferDto> GetTeamTransferOffers(Guid teamId)
        {
            return _context.PlayerTransferOffers.Where(x => x.ToTeamId == teamId && !x.IsAcceptedByOtherTeam && !x.IsAcceptedByPlayer).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).Select(x => new PlayerTransferOfferDto {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FromTeamId = x.FromTeamId,
                ToTeamId = x.ToTeamId,
                //Content = x.Content,
                TransferFee = x.TransferFee,
                //IsAcceptedByPlayer = x.IsAcceptedByPlayer,
                //IsAcceptedByOtherTeam = x.IsAcceptedByOtherTeam,
                //PlayerName = x.Player.FirstName + " " + x.Player.LastName,
                //FromTeamName = x.FromTeam.Name,
                //ToTeamName = x.ToTeam.Name
            }).ToList();
        }

        public List<PlayerTransferOfferDto> GetFromTeamTransferOffers(Guid teamId)
        {
            return _context.PlayerTransferOffers.Where(x => x.FromTeamId == teamId && !x.IsAcceptedByPlayer).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).Select(x => new PlayerTransferOfferDto {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FromTeamId = x.FromTeamId,
                ToTeamId = x.ToTeamId,
                //Content = x.Content,
                TransferFee = x.TransferFee,
                IsAcceptedByPlayer = x.IsAcceptedByPlayer,
                IsAcceptedByOtherTeam = x.IsAcceptedByOtherTeam,
                PlayerName = x.Player.FirstName + " " + x.Player.LastName,
                //FromTeamName = x.FromTeam.Name,
                //ToTeamName = x.ToTeam.Name
            }).ToList();
        }

        public List<PlayerTransferOffer> GetPlayerTransferOffers(Guid userId)
        {
            Player player = _context.Players.Single(x => x.UserId == userId);
            return _context.PlayerTransferOffers.Where(x => x.PlayerId == player.Id && x.IsAcceptedByOtherTeam && !x.IsAcceptedByPlayer).ToList();
        }

        public void AddPlayerTransferOffer(AddPlayerTransferOfferDto model)
        {
            PlayerTransferOffer playerTransferOffer = _mapper.Map<PlayerTransferOffer>(model);
            playerTransferOffer.IsAcceptedByOtherTeam = false;
            playerTransferOffer.IsAcceptedByPlayer = false;
            _context.PlayerTransferOffers.Add(playerTransferOffer);
            _context.SaveChanges();
        }
    }
}
