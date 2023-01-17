using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using SocialballWebAPI.ViewModels;
using SocialballWebAPI.Helpers;
using SocialballWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.DTOs.Teams;
using BCrypt.Net;

namespace SocialballWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            User user = _userRepository.GetUserDetailsByUsername(model.Username);
            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(model.Password, user.Password) ||
                user.UserData.UserType == UserType.System ||
                !user.IsActive)
            {
                return null;
            }
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetUserDetails(id);
        }

        public Guid AddUserAccountForNewPlayer(RegisterPlayerDto playerModel)
        {
            User user = new User()
            {
                Username = playerModel.LoginUsername,
                Password = BCrypt.Net.BCrypt.HashPassword(playerModel.LoginPassword),
                Email = playerModel.Email,
                IsActive = true
            };
            _userRepository.AddUser(user);

            return user.Id;
        }

        public Guid AddUserAccountForNewTeamManager(AddTeamDto model)
        {
            User user = new User()
            {
                Username = model.LoginUsername,
                Password = BCrypt.Net.BCrypt.HashPassword(model.LoginPassword),
                Email = model.Email,
                IsActive = true
            };
            _userRepository.AddUser(user);

            return user.Id;
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public List<SelectList> GetUsersToLookup()
        {
            List<SelectList> users = _userRepository.GetPlayersUsers().Select(x => new SelectList
            {
                Id = x.Id,
                Name = x.UserData.FirstName + " " + x.UserData.LastName
            }).ToList();

            return users;
        }

    }
}
