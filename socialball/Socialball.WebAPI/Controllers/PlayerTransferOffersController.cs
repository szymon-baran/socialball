using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerTransferOffersController : ControllerBase
    {
        private IPlayerTransferOfferService PlayerTransferOfferService;

        public PlayerTransferOffersController(IPlayerTransferOfferService service)
        {
            PlayerTransferOfferService = service;
        }

        [HttpGet("getTeamTransferOffers")]
        public ActionResult<IEnumerable<PlayerTransferOfferDto>> GetTeamTransferOffers(Guid teamId)
        {
            return Ok(PlayerTransferOfferService.GetTeamTransferOffers(teamId));
        }

        [HttpGet("getFromTeamTransferOffers")]
        public ActionResult<IEnumerable<PlayerTransferOfferDto>> GetFromTeamTransferOffers(Guid teamId)
        {
            return Ok(PlayerTransferOfferService.GetFromTeamTransferOffers(teamId));
        }

        [HttpGet("getPlayerTransferOffers")]
        public ActionResult<IEnumerable<PlayerTransferOffer>> GetPlayerTransferOffers(Guid userId)
        {
            return Ok(PlayerTransferOfferService.GetPlayerTransferOffers(userId));
        }

        [HttpPost("addPlayerTransferOffer")]
        [Authorize]
        public ActionResult AddPlayerTransferOffer(AddPlayerTransferOfferDto model)
        {
            PlayerTransferOfferService.AddPlayerTransferOffer(model);
            return Ok();
        }

        [HttpGet("getPlayerTransferOfferDetails")]
        public ActionResult<PlayerTransferOfferDto> GetPlayerTransferOfferDetails(Guid id)
        {
            return Ok(PlayerTransferOfferService.GetPlayerTransferOfferDetails(id));
        }

        [HttpPost("rejectOffer")]
        public ActionResult RejectOffer(GuidDto model)
        {
            PlayerTransferOfferService.RejectOffer(model.Id);
            return Ok();
        }

        [HttpPost("acceptOfferAsTeam")]
        public ActionResult AcceptOfferAsTeam(GuidDto model)
        {
            PlayerTransferOfferService.AcceptOfferAsTeam(model.Id);
            return Ok();
        }

        [HttpPost("acceptOfferAsPlayer")]
        public ActionResult AcceptOfferAsPlayer(GuidDto model)
        {
            PlayerTransferOfferService.AcceptOfferAsPlayer(model.Id);
            return Ok();
        }

    }
}
