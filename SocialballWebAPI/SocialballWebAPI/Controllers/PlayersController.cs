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
    public class PlayersController : ControllerBase
    {
        private IPlayerService PlayerService;

        public PlayersController(IPlayerService service)
        {
            PlayerService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Player>> GetPlayers(Guid? teamId)
        {
            if (teamId.HasValue)
            {
                return Ok(PlayerService.GetPlayersByTeamId(teamId.Value));
            }
            else
            {
                return Ok(PlayerService.GetPlayers());
            }
        }

        [Authorize]
        [HttpGet("getInjuredPlayers")]
        public ActionResult<IEnumerable<Player>> GetInjuredPlayers(Guid teamId)
        {
            return Ok(PlayerService.GetInjuredPlayers(teamId));
        }

        [HttpGet("details")]
        public ActionResult<GetPlayerDto> GetPlayer(Guid id)
        {
            return Ok(PlayerService.GetPlayerDetails(id));
        }

        [HttpGet("userDataDetails")]
        public ActionResult<GetUserDataDto> GetUserDataDetails(Guid id)
        {
            return Ok(PlayerService.GetUserDataDetails(id));
        }

        [HttpPost]
        public ActionResult PostPlayer([FromBody] RegisterPlayerDto playerModel)
        {
            PlayerService.AddPlayer(playerModel);

            return Ok();
        }

        [HttpPost("edit")]
        public ActionResult EditPlayer([FromBody] EditPlayerDto playerModel)
        {
            PlayerService.EditUserData(playerModel);

            return Ok();
        }

        [HttpPost("addEditPlayerInjury")]
        public ActionResult AddEditPlayerInjury([FromBody] PlayerInjuryDto model)
        {
            PlayerService.AddEditPlayerInjury(model);

            return Ok();
        }

        [HttpGet("isUsernameUnique")]
        public bool IsUsernameUnique(string username)
        {
            bool result = PlayerService.CheckUsernameUniqueness(username);
            return result;
        }

        [HttpGet("getPlayerByUserId")]
        public ActionResult<GetPlayerDto> GetPlayerByUserId(Guid userId)
        {
            return Ok(PlayerService.GetPlayerDetailsByUserId(userId));
        }

        [HttpGet("getUserDataByUserId")]
        public ActionResult<GetUserDataDto> GetUserDataByUserId(Guid userId)
        {
            return Ok(PlayerService.GetUserDataByUserId(userId));
        }

        [HttpGet("getUserTeamId")]
        public ActionResult<Guid?> GetUserTeamId(Guid userId)
        {
            return Ok(PlayerService.GetUserTeamId(userId));
        }

    }
}
