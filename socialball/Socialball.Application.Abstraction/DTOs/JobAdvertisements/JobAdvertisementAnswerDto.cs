using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class JobAdvertisementAnswerDto
    {
        public Guid? Id { get; set; }
        public Guid? JobAdvertisementId { get; set; }
        public JobAdvertisementType JobAdvertisementAnswerType { get; set; }
        public bool IsResponded { get; set; }
        public bool IsResponsePositive { get; set; }
        public string ResponseContent { get; set; }
        public string Content { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserId { get; set; }
        public string SenderName { get; set; }

    }
}
