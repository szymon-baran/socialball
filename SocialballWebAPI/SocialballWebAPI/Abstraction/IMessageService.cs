using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMessageService
    {
        object GetUserMessages(Guid userId);
        Message GetMessageDetails(Guid id);
        void AddMessage(SendMessageDto model);
        void MarkMessageAsRead(Guid id);
        void DeleteMessage(Guid id);
    }
}
