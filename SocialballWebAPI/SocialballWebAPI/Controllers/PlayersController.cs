using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly SocialballDBContext _context;

        public PlayersController(SocialballDBContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(Guid? teamId)
        {
            if (teamId.HasValue)
            {
                return await _context.Players.Where(x => x.TeamId == teamId).ToListAsync();
            }
            else
            {
                return await _context.Players.ToListAsync();
            }
        }

        // GET: api/Players/5
        [HttpGet("details")]
        public async Task<ActionResult<Player>> GetPlayer(Guid id)
        {
            var player = await _context.Players.Include(x => x.GoalsScored).FirstOrDefaultAsync(x => x.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer([FromBody] Player player)
        {
            if (!String.IsNullOrEmpty(player.LoginPassword))
            {
                player.LoginPassword = BCrypt.Net.BCrypt.HashPassword(player.LoginPassword);
            }
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(Guid id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
