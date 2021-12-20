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
            return _context.PlayerTransferOffers.Where(x => x.ToTeamId == teamId).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).Select(x => new PlayerTransferOfferDto
            {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FromTeamId = x.FromTeamId,
                ToTeamId = x.ToTeamId,
                //Content = x.Content,
                TransferFee = x.TransferFee,
                PlayerEarnings = x.PlayerEarnings,
                IsAcceptedByPlayer = x.IsAcceptedByPlayer,
                IsAcceptedByOtherTeam = x.IsAcceptedByOtherTeam,
                //PlayerName = x.Player.FirstName + " " + x.Player.LastName,
                //FromTeamName = x.FromTeam.Name,
                //ToTeamName = x.ToTeam.Name
            }).ToList();
        }

        public List<PlayerTransferOfferDto> GetFromTeamTransferOffers(Guid teamId)
        {
            return _context.PlayerTransferOffers.Where(x => x.FromTeamId == teamId).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).Select(x => new PlayerTransferOfferDto
            {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FromTeamId = x.FromTeamId,
                ToTeamId = x.ToTeamId,
                //Content = x.Content,
                TransferFee = x.TransferFee,
                PlayerEarnings = x.PlayerEarnings,
                IsAcceptedByPlayer = x.IsAcceptedByPlayer,
                IsAcceptedByOtherTeam = x.IsAcceptedByOtherTeam,
                PlayerName = x.Player.FirstName + " " + x.Player.LastName,
                //FromTeamName = x.FromTeam.Name,
                //ToTeamName = x.ToTeam.Name
            }).ToList();
        }

        public List<PlayerTransferOfferDto> GetPlayerTransferOffers(Guid userId)
        {
            Player player = _context.Players.Single(x => x.UserId == userId);
            return _context.PlayerTransferOffers.Where(x => x.PlayerId == player.Id).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).Select(x => new PlayerTransferOfferDto
            {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FromTeamId = x.FromTeamId,
                ToTeamId = x.ToTeamId,
                //Content = x.Content,
                TransferFee = x.TransferFee,
                PlayerEarnings = x.PlayerEarnings,
                IsAcceptedByPlayer = x.IsAcceptedByPlayer,
                IsAcceptedByOtherTeam = x.IsAcceptedByOtherTeam,
                //PlayerName = x.Player.FirstName + " " + x.Player.LastName,
                //FromTeamName = x.FromTeam.Name,
                //ToTeamName = x.ToTeam.Name
            }).ToList();
        }

        public void AddPlayerTransferOffer(AddPlayerTransferOfferDto model)
        {
            PlayerTransferOffer playerTransferOffer = _mapper.Map<PlayerTransferOffer>(model);
            playerTransferOffer.IsAcceptedByOtherTeam = false;
            playerTransferOffer.IsAcceptedByPlayer = false;
            _context.PlayerTransferOffers.Add(playerTransferOffer);
            _context.SaveChanges();
        }

        public PlayerTransferOfferDto GetPlayerTransferOfferDetails(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _context.PlayerTransferOffers.Include(x => x.FromTeam).Include(x => x.ToTeam).Include(x => x.Player).Single(x => x.Id == id);
            PlayerTransferOfferDto model = _mapper.Map<PlayerTransferOfferDto>(playerTransferOffer);
            model.PlayerName = playerTransferOffer.Player.FirstName + " " + playerTransferOffer.Player.LastName;
            model.FromTeamName = playerTransferOffer.FromTeam.Name;
            model.ToTeamName = playerTransferOffer.ToTeam.Name;
            return model;
        }

        public void RejectOffer(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _context.PlayerTransferOffers.Include(x => x.Player).Single(x => x.Id == id);

            Message systemMessage = new Message
            {
                FromUserId = _context.UserDatas.First(x => x.UserType == UserType.System).UserId,
                Subject = "Oferta transferowa odrzucona",
                Content = $"Oferta transferowa Twojej drużyny za zawodnika {playerTransferOffer.Player.FirstName ?? ""} {playerTransferOffer.Player.LastName} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                SentOn = DateTime.Now,
                MessageType = MessageType.System
            };
            _context.Messages.Add(systemMessage);
            _context.SaveChanges();

            List<UserData> teamManagers = _context.UserDatas.Include(x => x.User).ThenInclude(x => x.UserData).Where(x => x.TeamId == playerTransferOffer.FromTeamId && x.User.UserData.UserType == UserType.Management).ToList();
            foreach (var person in teamManagers)
            {
                if (!person.UserId.HasValue)
                {
                    continue;
                }

                UserMessage userMessage = new UserMessage()
                {
                    MessageId = systemMessage.Id,
                    ToUserId = person.UserId.Value
                };

                _context.UserMessages.Add(userMessage);
            }
            _context.PlayerTransferOffers.Remove(playerTransferOffer);
            _context.SaveChanges();

        }

        private void FinalizeTransfer(PlayerTransferOffer playerTransferOffer)
        {
            Player player = _context.Players.Single(x => x.Id == playerTransferOffer.PlayerId);
            player.TeamId = playerTransferOffer.FromTeamId;
            player.Earnings = playerTransferOffer.PlayerEarnings;
            _context.Players.Update(player);
            _context.PlayerTransferOffers.Remove(playerTransferOffer);
            _context.SaveChanges();
        }

        public void AcceptOfferAsTeam(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _context.PlayerTransferOffers.Single(x => x.Id == id);
            playerTransferOffer.IsAcceptedByOtherTeam = true;
            _context.PlayerTransferOffers.Update(playerTransferOffer);
            _context.SaveChanges();
            if (playerTransferOffer.IsAcceptedByPlayer)
            {
                FinalizeTransfer(playerTransferOffer);
            }
        }

        public void AcceptOfferAsPlayer(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _context.PlayerTransferOffers.Single(x => x.Id == id);
            playerTransferOffer.IsAcceptedByPlayer = true;
            _context.PlayerTransferOffers.Update(playerTransferOffer);
            _context.SaveChanges();
            if (playerTransferOffer.IsAcceptedByOtherTeam)
            {
                FinalizeTransfer(playerTransferOffer);
            }
        }
    }
}
