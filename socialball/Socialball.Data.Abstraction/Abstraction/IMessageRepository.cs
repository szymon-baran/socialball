using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMessageRepository
    {
        object GetUserMessages(Guid userId);
        object GetUserSentMessages(Guid userId);
        Message GetMessageDetails(Guid id);
        UserMessage GetUserMessageDetails(Guid id);
        void AddMessage(Message message);
        void AddUserMessage(UserMessage userMessage);
        void UpdateUserMessage(UserMessage userMessage);
    }
}
