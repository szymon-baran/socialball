using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Extensions;
using SocialballWebAPI.Models;
using SocialballWebAPI.Services;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private IMatchService MatchService;
        private IPlayerService PlayerService;

        public MatchesController(IMatchService matchService, IPlayerService playerService)
        {
            MatchService = matchService;
            PlayerService = playerService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Match>> GetMatches(Guid? teamId)
        {
            if (teamId.HasValue)
            {
                return Ok(MatchService.GetTeamMatches(teamId.Value));
            }
            else
            {
                return Ok(MatchService.GetAllMatches());
            }
        }

        [HttpGet("getUnconfirmedMatches")]
        [Authorize]
        public ActionResult<IEnumerable<Match>> GetUnconfirmedMatches(Guid teamId)
        {
            return Ok(MatchService.GetUnconfirmedMatches(teamId));
        }

        [HttpPost("addMatch")]
        [Authorize]
        public ActionResult AddMatch([FromBody] MatchDto model)
        {
            MatchService.AddMatch(model);

            return Ok();
        }

        [HttpGet("details")]
        public ActionResult<Match> GetMatch(Guid id)
        {
            return Ok(MatchService.GetMatchDetails(id));
        }


        [HttpPost("sendMatchAnswer")]
        [Authorize]
        public ActionResult SendMatchAnswer([FromBody] MatchAnswerDto model)
        {
            MatchService.SendMatchAnswer(model);

            return Ok();
        }

        [HttpGet("getPlayersByTeam")]
        public ActionResult GetPlayersByTeam(Guid teamId)
        {
            return Ok(PlayerService.GetPlayersByTeamToLookup(teamId));
        }

        [HttpGet("getEventTypesToLookup")]
        public ActionResult GetEventTypesToLookup()
        {
            return Ok(EnumExtensions.GetValues<MatchEventType>());
        }

        [HttpGet("getPenaltyTypesToLookup")]
        public ActionResult GetPenaltyTypesToLookup()
        {
            return Ok(EnumExtensions.GetValues<PenaltyType>());
        }

        [HttpGet("getMatchTypesToLookup")]
        public ActionResult GetMatchTypesToLookup()
        {
            return Ok(EnumExtensions.GetValues<MatchType>());
        }

    }
}
