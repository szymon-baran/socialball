using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Data.Repositories;
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
        private readonly IMessageRepository _messageRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IPlayerRepository playerRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public object GetUserMessages(Guid userId)
        {
            return _messageRepository.GetUserMessages(userId);
        }

        public object GetUserSentMessages(Guid userId)
        {
            return _messageRepository.GetUserSentMessages(userId);
        }

        public Message GetMessageDetails(Guid id)
        {
            Message message = _messageRepository.GetMessageDetails(id);
            if (message == null)
            {
                throw new KeyNotFoundException();
            }

            return new Message();
        }

        public void AddMessage(SendMessageDto model)
        {
            Message message = _mapper.Map<Message>(model);
            message.SentOn = DateTime.Now;

            _messageRepository.AddMessage(message);

            if (model.MessageType == MessageType.Private && model.ToUserId.HasValue)
            {
                UserMessage userMessage = new UserMessage()
                {
                    MessageId = message.Id,
                    ToUserId = model.ToUserId.Value
                };

                _messageRepository.AddUserMessage(userMessage);
            }
            else if (model.MessageType == MessageType.InsideTeam && model.ToTeamId.HasValue)
            {
                List<UserData> peopleInTeam = _playerRepository.GetUserDatasInTeam(model.ToTeamId.Value);
                foreach (var person in peopleInTeam)
                {
                    if (!person.UserId.HasValue || person.UserId == model.FromUserId)
                    {
                        continue;
                    }

                    UserMessage userMessage = new UserMessage()
                    {
                        MessageId = message.Id,
                        ToUserId = person.UserId.Value
                    };

                    _messageRepository.AddUserMessage(userMessage);
                }
            }
            else if (model.MessageType == MessageType.ToOtherTeam && model.ToTeamId.HasValue)
            {
                List<UserData> teamManagersInTeam = _playerRepository.GetManagersInTeam(model.ToTeamId.Value);
                foreach (var person in teamManagersInTeam)
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

                    _messageRepository.AddUserMessage(userMessage);
                }
            }
        }

        public void MarkMessageAsRead(Guid id)
        {
            UserMessage userMessage = _messageRepository.GetUserMessageDetails(id);
            userMessage.IsRead = true;
            _messageRepository.UpdateUserMessage(userMessage);
        }

        public void DeleteMessage(Guid id)
        {
            UserMessage userMessage = _messageRepository.GetUserMessageDetails(id);
            userMessage.IsActive = false;
            _messageRepository.UpdateUserMessage(userMessage);
        }

    }
}
