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
            TeamMessages = new HashSet<TeamMessage>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<TeamMessage> TeamMessages { get; set; }
    }
}
