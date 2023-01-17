using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly SocialballDBContext _context;

        public MessageRepository(SocialballDBContext context)
        {
            _context = context;
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
            return _context.Messages.Single(x => x.Id == id);
        }

        public UserMessage GetUserMessageDetails(Guid id)
        {
            return _context.UserMessages.Single(x => x.Id == id);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public void AddUserMessage(UserMessage userMessage)
        {
            _context.UserMessages.Add(userMessage);
            _context.SaveChanges();
        }

        public void UpdateUserMessage(UserMessage userMessage)
        {
            _context.UserMessages.Update(userMessage);
            _context.SaveChanges();
        }

    }
}
