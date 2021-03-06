using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.Teams;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Extensions;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private ITeamService TeamService;

        public TeamsController(ITeamService service)
        {
            TeamService = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Team>> GetTeams()
        {
            return Ok(TeamService.GetTeams());
        }

        [HttpPost]
        public ActionResult AddTeam([FromBody] AddTeamDto model)
        {
            TeamService.AddTeam(model);

            return Ok();
        }

        [HttpGet("getTeamsByLeague")]
        public ActionResult<IEnumerable<Team>> GetTeamsByLeague(Guid leagueId)
        {
            return Ok(TeamService.GetTeamsByLeague(leagueId));
        }

        [HttpGet("selectList")]
        public ActionResult GetTeamsToSelectList()
        {
            return Ok(TeamService.GetTeamsToSelectList());
        }

        [HttpGet("getPositionsInsideOfTeam")]
        public ActionResult GetPositionsInsideOfTeam(Guid teamId)
        {
            return Ok(TeamService.GetPositionsCountToChart(teamId));
        }

        [HttpGet("details")]
        public ActionResult<TeamDto> GetTeamDetails(Guid id)
        {
            return Ok(TeamService.GetTeamDetails(id));
        }

        [HttpGet("getPositionsToLookup")]
        public ActionResult GetPositionsToLookup()
        {
            return Ok(EnumExtensions.GetValues<PositionType>());
        }

        [HttpGet("getLeaguesToLookup")]
        public ActionResult GetLeaguesToLookup()
        {
            return Ok(TeamService.GetLeaguesToLookup());
        }
    }
}
