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
using SocialballWebAPI.DTOs.Admins;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private IAdminService AdminService;

        public AdminsController(IAdminService service)
        {
            AdminService = service;
        }

        [HttpGet("getUsersList")]
        public ActionResult<IEnumerable<Player>> GetPlayers()
        {
            return Ok(AdminService.GetUsers());
        }

        [HttpGet("getTeamsList")]
        public ActionResult<IEnumerable<Player>> GetTeams(Guid? leagueId)
        {
            return Ok(AdminService.GetTeams(leagueId));
        }

        [HttpPost("teamEdit")]
        public ActionResult EditTeam([FromBody] AdminEditTeamDto model)
        {
            AdminService.EditTeam(model);

            return Ok();
        }

        [HttpPost("teamDeleteAdmin")]
        public ActionResult TeamDeleteAdmin(GuidDto model)
        {
            AdminService.TeamDeleteAdmin(model.Id);
            return Ok();
        }

        [HttpPost("teamImageDeleteAdmin")]
        public ActionResult TeamImageDeleteAdmin(GuidDto model)
        {
            AdminService.TeamImageDeleteAdmin(model.Id);
            return Ok();
        }
    }
}
