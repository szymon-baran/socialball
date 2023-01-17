using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public abstract class JobAdvertisementAnswer
    {
        public Guid Id { get; set; }
        public Guid? JobAdvertisementId { get; set; }
        public Guid? TeamId { get; set; }
        public JobAdvertisementType JobAdvertisementAnswerType { get; set; }
        public string Content { get; set; }
        public bool IsResponded { get; set; }
        public bool IsResponsePositive { get; set; }
        public string ResponseContent { get; set; }
        public Team? Team { get; set; }
        public JobAdvertisement? JobAdvertisement { get; set; }
    }
}
