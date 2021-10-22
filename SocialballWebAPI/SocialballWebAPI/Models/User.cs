using System;
using System.Text.Json.Serialization;

namespace SocialballWebAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public virtual Player Player { get; set; }
    }
}
