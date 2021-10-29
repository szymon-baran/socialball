using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Extensions;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public MessagesController(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/TeamMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(Guid userId)
        {
            var playerDetails = _context.Players.Single(x => x.UserId == userId);
            return await _context.Messages.Where(x => x is PrivateMessage ? ((PrivateMessage)x).ToUserId == userId : (x is TeamMessage ? ((TeamMessage)x).ToTeamId == playerDetails.TeamId : false)).ToListAsync();
        }

        // GET: api/TeamMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(Guid id)
        {
            var teamMessage = await _context.Messages.FindAsync(id);

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
        public async Task<IActionResult> PutMessage(Guid id, Message teamMessage)
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
                if (!MessageExists(id))
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

        [HttpPost("addTeamMessage")]
        public async Task<ActionResult<Message>> AddTeamMessage(SendTeamMessageDto model)
        {
            TeamMessage teamMessage = _mapper.Map<TeamMessage>(model);
            teamMessage.SentOn = DateTime.Now;

            _context.Messages.Add(teamMessage);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/TeamMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(Guid id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }

        [HttpGet("getMessageTypesToLookup")]
        public ActionResult GetMessageTypesToLookup()
        {
            return Ok(EnumExtensions.GetValues<MessageType>());
        }
    }
}
