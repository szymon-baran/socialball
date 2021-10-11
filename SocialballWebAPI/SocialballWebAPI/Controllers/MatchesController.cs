﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly SocialballDBContext _context;

        public MatchesController(SocialballDBContext context)
        {
            _context = context;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches(Guid? teamId)
        {
            if (teamId.HasValue)
            {
                return await _context.Matches.Where(x => x.HomeTeamId == teamId || x.AwayTeamId == teamId).Include(x => x.Goals).ThenInclude(x => x.Scorer).ToListAsync();
            }
            else
            {
                return await _context.Matches.Include(x => x.Goals).ThenInclude(x => x.Scorer).ToListAsync();
            }
        }

        // GET: api/Matches/5
        [HttpGet("details")]
        public async Task<ActionResult<Match>> GetMatch(Guid id)
        {
            var match = await _context.Matches
                .Include(x => x.Goals.OrderBy(y => y.Minute))
                    .ThenInclude(x => x.Scorer)
                .Include(x => x.Goals.OrderBy(y => y.Minute))
                    .ThenInclude(x => x.AssistPlayer)
                .Include(x => x.AwayTeam)
                    .ThenInclude(x => x.League)
                .Include(x => x.HomeTeam)
                    .ThenInclude(x => x.League)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        // PUT: api/Matches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(Guid id, Match match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
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

        // POST: api/Matches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Match>> PostMatch(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, match);
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(Guid id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MatchExists(Guid id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
