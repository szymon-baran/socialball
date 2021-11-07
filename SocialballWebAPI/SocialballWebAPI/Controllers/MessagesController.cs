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
using SocialballWebAPI.ViewModels;

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
        public async Task<ActionResult<IEnumerable<UserMessage>>> GetMessages(Guid userId)
        {
            return await _context.UserMessages.Include(x => x.Message).Where(x => x.ToUserId == userId).ToListAsync();
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

        [HttpPost("addMessage")]
        public async Task<ActionResult<Message>> AddTeamMessage(SendMessageDto model)
        {
            Message message = _mapper.Map<Message>(model);
            message.SentOn = DateTime.Now;

            _context.Messages.Add(message);
            _context.SaveChanges();

            if (model.MessageType == MessageType.Prywatna && model.ToUserId.HasValue)
            {
                UserMessage userMessage = new UserMessage()
                {
                    MessageId = message.Id,
                    ToUserId = model.ToUserId.Value
                };

                _context.UserMessages.Add(userMessage);
            }
            else if (model.MessageType == MessageType.Druzynowa && model.ToTeamId.HasValue)
             {
                List<UserData> peopleInTeam = _context.UserDatas.Where(x => x.TeamId == model.ToTeamId.Value).ToList();
                foreach (var person in peopleInTeam)
                {
                    if (!person.UserId.HasValue)
                    {
                        continue;
                    }

                    UserMessage userMessage = new UserMessage()
                    {
                        MessageId = message.Id,
                        ToUserId = person.UserId.Value
                    };

                    _context.UserMessages.Add(userMessage);
                }
            }

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

        [HttpPost("markMessageAsRead")]
        public ActionResult MarkMessageAsRead(GuidDto model)
        {
            UserMessage userMessage = _context.UserMessages.Single(x => x.Id == model.Id);
            userMessage.IsRead = true;
            _context.SaveChanges();

            return Ok(true);
        }

        [HttpGet("getUsersToLookup")]
        public ActionResult GetUsersToLookup()
        {
            List<SelectList> users = _context.Users.Select(x => new SelectList
            {
                Id = x.Id,
                Name = x.UserData.FirstName + " " + x.UserData.LastName
            }).ToList();

            return Ok(users);
        }
    }
}
