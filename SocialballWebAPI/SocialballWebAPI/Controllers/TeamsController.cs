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

        [HttpGet("selectList")]
        public ActionResult GetTeamsToSelectList()
        {
            return Ok(TeamService.GetTeamsToSelectList()); 
        }

        [HttpGet("getPositionsInsideOfTeam")]
        public ActionResult GetPositionsInsideOfTeam(Guid teamId)
        {
            return Ok(TeamService.GetTeamsToChart(teamId));
        }

        [HttpGet("{id}")]
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
