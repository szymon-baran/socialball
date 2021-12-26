using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialballWebAPI.Models
{
    public class User
    {
        public User()
        {
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<UserMessage>();
            FromUserJobAdvertisements = new HashSet<FromUserJobAdvertisement>();
            JobAdvertisementUserAnswers = new HashSet<JobAdvertisementUserAnswer>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public virtual UserData UserData { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<UserMessage> ReceivedMessages { get; set; }
        public virtual ICollection<FromUserJobAdvertisement> FromUserJobAdvertisements { get; set; }
        public virtual ICollection<JobAdvertisementUserAnswer> JobAdvertisementUserAnswers { get; set; }
    }
}
