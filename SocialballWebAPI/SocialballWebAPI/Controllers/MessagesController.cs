using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class MessagesController : ControllerBase
    {
        private IMessageService MessageService;
        private IUserService UserService;

        public MessagesController(IMessageService messageService, IUserService userService)
        {
            MessageService = messageService;
            UserService = userService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<UserMessage>> GetMessages(Guid userId)
        {
            return Ok(MessageService.GetUserMessages(userId));
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Message> GetMessage(Guid id)
        {
            return Ok(MessageService.GetMessageDetails(id));
        }

        [HttpPost("addMessage")]
        [Authorize]
        public ActionResult AddTeamMessage(SendMessageDto model)
        {
            MessageService.AddMessage(model);
            return Ok();
        }

        [HttpGet("getMessageTypesToLookup")]
        public ActionResult GetMessageTypesToLookup()
        {
            return Ok(EnumExtensions.GetValues<MessageType>());
        }

        [HttpPost("markMessageAsRead")]
        public ActionResult MarkMessageAsRead(GuidDto model)
        {
            MessageService.MarkMessageAsRead(model.Id);
            return Ok();
        }

        [HttpGet("getUsersToLookup")]
        public ActionResult GetUsersToLookup()
        {
            List<SelectList> users = UserService.GetUsersToLookup();
            return Ok(users);
        }
    }
}
