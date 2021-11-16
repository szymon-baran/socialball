using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;

namespace SocialballWebAPI.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public UserType UserType { get; set; }
        public Guid? TeamId { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            UserType = user.UserData.UserType;
            TeamId = user.UserData.TeamId;
            Token = token;
        }
    }
}
