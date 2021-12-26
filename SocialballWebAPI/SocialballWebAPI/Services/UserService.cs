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

namespace SocialballWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly SocialballDBContext _context;
        private readonly AppSettings _appSettings;

        public UserService(SocialballDBContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.Include(x => x.UserData).SingleOrDefault(x => x.Username == model.Username);

            // return null if user not found
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) return null;

            if (user.UserData.UserType == UserType.System || !user.IsActive) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public Guid AddUserAccountForNewPlayer(RegisterPlayerDto playerModel)
        {
            User user = new User()
            {
                Username = playerModel.LoginUsername,
                Password = BCrypt.Net.BCrypt.HashPassword(playerModel.LoginPassword),
                Email = playerModel.Email
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return user.Id;
        }

        public Guid AddUserAccountForNewTeamManager(AddTeamDto model)
        {
            User user = new User()
            {
                Username = model.LoginUsername,
                Password = BCrypt.Net.BCrypt.HashPassword(model.LoginPassword),
                Email = model.Email
            };
            _context.Users.Add(user);
            _context.SaveChanges();

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
            List<SelectList> users = _context.Users.Include(x => x.UserData).Where(x => x.UserData.UserType == UserType.Player).Select(x => new SelectList
            {
                Id = x.Id,
                Name = x.UserData.FirstName + " " + x.UserData.LastName
            }).ToList();

            return users;
        }

    }
}
