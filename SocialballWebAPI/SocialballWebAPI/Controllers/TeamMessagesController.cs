using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMessagesController : ControllerBase
    {
        private readonly SocialballDBContext _context;

        public TeamMessagesController(SocialballDBContext context)
        {
            _context = context;
        }

        // GET: api/TeamMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamMessage>>> GetTeamMessages()
        {
            return await _context.TeamMessages.ToListAsync();
        }

        // GET: api/TeamMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamMessage>> GetTeamMessage(Guid id)
        {
            var teamMessage = await _context.TeamMessages.FindAsync(id);

            if (teamMessage == null)
            {
                return NotFound();
            }

            return teamMessage;
        }

        // PUT: api/TeamMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTeamMessage(Guid id, TeamMessage teamMessage)
        {
            if (id != teamMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(teamMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamMessageExists(id))
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

        // POST: api/TeamMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeamMessage>> PostTeamMessage(TeamMessage teamMessage)
        {
            _context.TeamMessages.Add(teamMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeamMessage", new { id = teamMessage.Id }, teamMessage);
        }

        // DELETE: api/TeamMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamMessage(Guid id)
        {
            var teamMessage = await _context.TeamMessages.FindAsync(id);
            if (teamMessage == null)
            {
                return NotFound();
            }

            _context.TeamMessages.Remove(teamMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamMessageExists(Guid id)
        {
            return _context.TeamMessages.Any(e => e.Id == id);
        }
    }
}
