using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public MessageService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public object GetUserMessages(Guid userId)
        {
            return _context.UserMessages.Include(x => x.Message).ThenInclude(x => x.FromUser).ThenInclude(x => x.UserData).Where(x => x.IsActive && x.ToUserId == userId).ToList();
        }

        public object GetUserSentMessages(Guid userId)
        {
            return _context.UserMessages.Include(x => x.ToUser).ThenInclude(x => x.UserData).Include(x => x.Message).Where(x => x.Message.FromUserId == userId).ToList();
        }

        public Message GetMessageDetails(Guid id)
        {
            Message message = _context.Messages.Single(x => x.Id == id);
            if (message == null)
            {
                throw new KeyNotFoundException();
            }

            return message;
        }

        public void AddMessage(SendMessageDto model)
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
            _context.SaveChanges();
        }

        public void MarkMessageAsRead(Guid id)
        {
            UserMessage userMessage = _context.UserMessages.Single(x => x.Id == id);
            userMessage.IsRead = true;
            _context.SaveChanges();
        }

        public void DeleteMessage(Guid id)
        {
            UserMessage userMessage = _context.UserMessages.Single(x => x.Id == id);
            userMessage.IsActive = false;
            _context.SaveChanges();
        }

    }
}
