using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public TeamsController(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.Include(x => x.League).ToListAsync();
        }

        [HttpGet("selectList")]
        public async Task<List<SelectList>> GetTeamsToSelectList()
        {
            return await _context.Teams.Select(x => new SelectList { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        [HttpGet("getPositionsInsideOfTeam")]
        public async Task<List<PositionsInTeam>> GetPositionsInsideOfTeam(Guid teamId)
        {
            List<PositionsInTeam> positionsInTeams = new List<PositionsInTeam>();

            for (PositionType p = 0; p <= PositionType.Napastnik; p++)
            {
                positionsInTeams.Add(new PositionsInTeam
                {
                    Position = p,
                    NumberOfPlayers = _context.Players.Where(x => x.TeamId == teamId && x.Position == (int)p).Count()
                });
            }

            return positionsInTeams;
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            TeamDto model = _mapper.Map<TeamDto>(team);

            return model;
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(Guid id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(Guid id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        // lookups
        [HttpGet("getPositionsToLookup")]
        public ActionResult GetPositionsToLookup()
        {
            return Ok(EnumExtensions.GetValues<PositionType>());
        }

        [HttpGet("getLeaguesToLookup")]
        public ActionResult GetLeaguesToLookup()
        {
            return Ok(_context.Leagues.ToList());
        }
    }
}
