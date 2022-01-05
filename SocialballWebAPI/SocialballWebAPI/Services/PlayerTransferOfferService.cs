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
        private readonly IMapper _mapper;
        private readonly IPlayerTransferOfferRepository _playerTransferOfferRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMessageRepository _messageRepository;

        public PlayerTransferOfferService(IMapper mapper, IPlayerTransferOfferRepository playerTransferOfferRepository, IPlayerRepository playerRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _playerTransferOfferRepository = playerTransferOfferRepository;
            _playerRepository = playerRepository;
            _messageRepository = messageRepository;
        }

        public List<PlayerTransferOfferDto> GetTeamTransferOffers(Guid teamId)
        {
            return _playerTransferOfferRepository.GetToTeamTransferOffers(teamId).Select(x => new PlayerTransferOfferDto
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
            return _playerTransferOfferRepository.GetFromTeamTransferOffers(teamId).Select(x => new PlayerTransferOfferDto
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
            Player player = _playerRepository.GetPlayerDetailsByUserId(userId);
            return _playerTransferOfferRepository.GetPlayerTransferOffers(player.Id).Select(x => new PlayerTransferOfferDto
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
            _playerTransferOfferRepository.AddPlayerTransferOffer(playerTransferOffer);
        }

        public PlayerTransferOfferDto GetPlayerTransferOfferDetails(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _playerTransferOfferRepository.GetPlayerTransferOfferDetails(id);
            PlayerTransferOfferDto model = _mapper.Map<PlayerTransferOfferDto>(playerTransferOffer);
            model.PlayerName = playerTransferOffer.Player.FirstName + " " + playerTransferOffer.Player.LastName;
            model.FromTeamName = playerTransferOffer.FromTeam.Name;
            model.ToTeamName = playerTransferOffer.ToTeam.Name;
            return model;
        }

        public void RejectOffer(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _playerTransferOfferRepository.GetPlayerTransferOfferDetails(id);

            Message systemMessage = new Message
            {
                FromUserId = _playerRepository.GetSystemUserDetails().UserId,
                Subject = "Oferta transferowa odrzucona",
                Content = $"Oferta transferowa Twojej drużyny za zawodnika {playerTransferOffer.Player.FirstName ?? ""} {playerTransferOffer.Player.LastName} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                SentOn = DateTime.Now,
                MessageType = MessageType.System
            };
            _messageRepository.AddMessage(systemMessage);

            List<UserData> teamManagers = _playerRepository.GetManagersInTeam(playerTransferOffer.FromTeamId).ToList();
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

                _messageRepository.AddUserMessage(userMessage);
            }

            _playerTransferOfferRepository.RemovePlayerTransferOffer(playerTransferOffer);
        }

        private void FinalizeTransfer(PlayerTransferOffer playerTransferOffer)
        {
            Player player = _playerRepository.GetPlayerDetails(playerTransferOffer.PlayerId);
            player.TeamId = playerTransferOffer.FromTeamId;
            player.Earnings = playerTransferOffer.PlayerEarnings;
            _playerRepository.UpdatePlayer(player);
            _playerTransferOfferRepository.RemovePlayerTransferOffer(playerTransferOffer);
        }

        public void AcceptOfferAsTeam(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _playerTransferOfferRepository.GetPlayerTransferOfferDetails(id);
            playerTransferOffer.IsAcceptedByOtherTeam = true;
            _playerTransferOfferRepository.UpdatePlayerTransferOffer(playerTransferOffer);
            if (playerTransferOffer.IsAcceptedByPlayer)
            {
                FinalizeTransfer(playerTransferOffer);
            }
        }

        public void AcceptOfferAsPlayer(Guid id)
        {
            PlayerTransferOffer playerTransferOffer = _playerTransferOfferRepository.GetPlayerTransferOfferDetails(id);
            playerTransferOffer.IsAcceptedByPlayer = true;
            _playerTransferOfferRepository.UpdatePlayerTransferOffer(playerTransferOffer);
            if (playerTransferOffer.IsAcceptedByOtherTeam)
            {
                FinalizeTransfer(playerTransferOffer);
            }
        }
    }
}
