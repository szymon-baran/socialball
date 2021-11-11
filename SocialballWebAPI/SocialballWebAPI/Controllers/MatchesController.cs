using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private IMatchService MatchService;

        public MatchesController(IMatchService service)
        {
            MatchService = service;
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

        [HttpGet("details")]
        public ActionResult<Match> GetMatch(Guid id)
        {
            return Ok(MatchService.GetMatchDetails(id));
        }

    }
}
